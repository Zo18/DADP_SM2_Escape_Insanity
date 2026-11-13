using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class SetupLilithaEnemy
{
    private const string ScenePath = "Assets/Scenes/LevelDesignScene.unity";
    private const string ModelPath = "Assets/Models/Lilitha_Walk.fbx";
    private const string ColourTexturePath = "Assets/Models/Lilitha_Colour.png";
    private const string MaterialPath = "Assets/Models/Lilitha_BlackBody_RedFace.mat";
    private const string ControllerPath = "Assets/Models/Lilitha_Walk.controller";
    private const string EnemyName = "Lilitha_Walk_Enemy";
    private const float EnemyScale = 0.14f;

    static SetupLilithaEnemy() => EditorApplication.delayCall += Setup;

    public static void Setup()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;
        Scene scene = default;
        bool openedAdditively = false;
        try
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            if (!AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath)) throw new System.Exception("LevelDesignScene could not be found.");

            // The supplied FBX has animation data but no Avatar configured. Use a
            // generic rig so its authored walk clip can drive the imported skeleton.
            ModelImporter modelImporter = AssetImporter.GetAtPath(ModelPath) as ModelImporter;
            if (modelImporter != null &&
                (modelImporter.animationType != ModelImporterAnimationType.Generic ||
                 modelImporter.avatarSetup != ModelImporterAvatarSetup.CreateFromThisModel))
            {
                modelImporter.animationType = ModelImporterAnimationType.Generic;
                modelImporter.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
                modelImporter.SaveAndReimport();
            }

            if (SceneManager.GetSceneByPath(ScenePath).isLoaded)
                scene = SceneManager.GetSceneByPath(ScenePath);
            else
            {
                scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Additive);
                openedAdditively = true;
            }

            Transform[] transforms = scene.GetRootGameObjects()
                .SelectMany(root => root.GetComponentsInChildren<Transform>(true)).ToArray();
            Transform player = transforms.FirstOrDefault(t => t.CompareTag("Player"));
            if (player == null) throw new System.Exception("No Player tagged object was found in LevelDesignScene.");
            SanitySystem sanity = player.GetComponent<SanitySystem>();
            if (sanity == null) sanity = player.gameObject.AddComponent<SanitySystem>();
            sanity.playerMovement = player.GetComponent<FPController>();

            GameObject enemy = transforms.Select(t => t.gameObject).FirstOrDefault(go => go.name == EnemyName);
            if (enemy == null)
            {
                GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(ModelPath);
                if (model == null) throw new System.Exception("Lilitha_Walk.fbx did not import as a model prefab.");
                enemy = (GameObject)PrefabUtility.InstantiatePrefab(model, scene);
                enemy.name = EnemyName;
                enemy.transform.position = player.position + player.forward * 12f + Vector3.up * 0.02f;
                enemy.transform.rotation = Quaternion.LookRotation(player.position - enemy.transform.position, Vector3.up);
            }

            CapsuleCollider capsule = enemy.GetComponent<CapsuleCollider>();
            if (capsule == null) capsule = enemy.AddComponent<CapsuleCollider>();
            enemy.transform.localScale = Vector3.one * EnemyScale;
            capsule.isTrigger = true;
            // Lilitha's FBX is authored at about 12.4 Unity units tall; scale
            // the collider's local dimensions so it remains human sized too.
            capsule.center = new Vector3(0f, 6.1f, 0f);
            capsule.height = 12.2f;
            capsule.radius = 2.15f;

            Texture2D colourTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(ColourTexturePath);
            if (colourTexture == null) throw new System.Exception("Lilitha_Colour.png could not be imported.");
            Material colourMaterial = AssetDatabase.LoadAssetAtPath<Material>(MaterialPath);
            if (colourMaterial == null)
            {
                Shader shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
                colourMaterial = new Material(shader) { name = "Lilitha_BlackBody_RedFace" };
                AssetDatabase.CreateAsset(colourMaterial, MaterialPath);
            }
            if (colourMaterial.HasProperty("_BaseMap")) colourMaterial.SetTexture("_BaseMap", colourTexture);
            if (colourMaterial.HasProperty("_MainTex")) colourMaterial.SetTexture("_MainTex", colourTexture);
            if (colourMaterial.HasProperty("_BaseColor")) colourMaterial.SetColor("_BaseColor", Color.white);
            if (colourMaterial.HasProperty("_Color")) colourMaterial.SetColor("_Color", Color.white);
            foreach (Renderer renderer in enemy.GetComponentsInChildren<Renderer>(true))
            {
                Material[] materials = renderer.sharedMaterials;
                for (int i = 0; i < materials.Length; i++) materials[i] = colourMaterial;
                renderer.sharedMaterials = materials;
            }
            EditorUtility.SetDirty(colourMaterial);

            Animator animator = enemy.GetComponentInChildren<Animator>();
            if (animator == null) animator = enemy.AddComponent<Animator>();
            animator.applyRootMotion = false;
            Avatar avatar = AssetDatabase.LoadAllAssetsAtPath(ModelPath).OfType<Avatar>().FirstOrDefault();
            if (avatar != null) animator.avatar = avatar;

            AnimationClip walk = AssetDatabase.LoadAllAssetsAtPath(ModelPath).OfType<AnimationClip>()
                .FirstOrDefault(clip => clip.name.ToLowerInvariant().Contains("walk"))
                ?? AssetDatabase.LoadAllAssetsAtPath(ModelPath).OfType<AnimationClip>().FirstOrDefault();
            if (walk != null)
            {
                AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(ControllerPath);
                if (controller == null)
                {
                    controller = AnimatorController.CreateAnimatorControllerAtPath(ControllerPath);
                    AnimatorState state = controller.layers[0].stateMachine.AddState("Lilitha_Walk");
                    controller.layers[0].stateMachine.defaultState = state;
                }
                AnimatorState walkAnimatorState = controller.layers[0].stateMachine.states
                    .Select(child => child.state).FirstOrDefault(state => state.name == "Lilitha_Walk");
                if (walkAnimatorState != null) walkAnimatorState.motion = walk;
                animator.runtimeAnimatorController = controller;
            }

            EnemyChase chase = enemy.GetComponent<EnemyChase>();
            if (chase == null) chase = enemy.AddComponent<EnemyChase>();
            SerializedObject serialized = new SerializedObject(chase);
            serialized.FindProperty("target").objectReferenceValue = player;
            if (animator != null) serialized.FindProperty("animator").objectReferenceValue = animator;
            serialized.FindProperty("sanitySystem").objectReferenceValue = sanity;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            Debug.Log("Lilitha_Walk is now the chasing enemy in LevelDesignScene; the caught screen displays the game-over prompt.");
        }
        catch (System.Exception exception)
        {
            Debug.LogError("Lilitha enemy setup failed: " + exception);
        }
        finally
        {
            if (openedAdditively && scene.IsValid() && scene.isLoaded)
                EditorSceneManager.CloseScene(scene, true);
        }
    }
}
