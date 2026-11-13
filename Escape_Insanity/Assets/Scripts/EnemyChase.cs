using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public sealed class EnemyChase : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField, Min(0f)] private float moveSpeed = 2.5f;
    [SerializeField, Min(0.1f)] private float catchDistance = 1.1f;
    [SerializeField] private Animator animator;
    [SerializeField] private string walkState = "Lilitha_Walk";
    [SerializeField] private Material appearanceMaterial;
    [SerializeField] private SanitySystem sanitySystem;

    private bool caughtPlayer;
    private bool chaseActivated;
    private GUIStyle titleStyle;
    private GUIStyle promptStyle;

    private void Awake()
    {
        Time.timeScale = 1f;
        if (appearanceMaterial != null)
        {
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>(true))
            {
                Material[] materials = renderer.sharedMaterials;
                for (int i = 0; i < materials.Length; i++) materials[i] = appearanceMaterial;
                renderer.sharedMaterials = materials;
            }
        }
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }
        if (target != null)
        {
            sanitySystem = target.GetComponent<SanitySystem>();
            if (sanitySystem == null) sanitySystem = target.gameObject.AddComponent<SanitySystem>();
            sanitySystem.playerMovement = target.GetComponent<FPController>();
        }
        if (animator != null) animator.speed = 0f;
    }

    private void Update()
    {
        if (caughtPlayer)
        {
            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            return;
        }

        if (target == null) return;
        if (!chaseActivated)
        {
            if (sanitySystem == null || !sanitySystem.HasLost) return;
            chaseActivated = true;
            if (animator != null)
            {
                animator.speed = 1f;
                if (!string.IsNullOrEmpty(walkState)) animator.Play(walkState, 0, 0f);
            }
        }

        Vector3 toPlayer = target.position - transform.position;
        toPlayer.y = 0f;
        float distance = toPlayer.magnitude;
        if (distance <= catchDistance)
        {
            CatchPlayer();
            return;
        }
        if (distance <= 0.001f) return;

        Vector3 direction = toPlayer / distance;
        transform.position += direction * (moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.LookRotation(direction, Vector3.up),
            240f * Time.deltaTime);
    }

    private void CatchPlayer()
    {
        caughtPlayer = true;
        if (target.TryGetComponent(out FPController controller)) controller.enabled = false;
        if (target.TryGetComponent(out PlayerInput playerInput)) playerInput.enabled = false;
        if (target.TryGetComponent(out PlayerInteract interaction)) interaction.enabled = false;

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnGUI()
    {
        if (!caughtPlayer) return;
        float width = Mathf.Min(600f, Screen.width - 40f);
        float height = 260f;
        Rect panel = new Rect((Screen.width - width) * 0.5f, (Screen.height - height) * 0.5f, width, height);
        GUI.color = new Color(0.025f, 0.015f, 0.02f, 0.94f);
        GUI.DrawTexture(panel, Texture2D.whiteTexture);
        GUI.color = Color.white;

        titleStyle ??= new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = Mathf.Clamp(Screen.height / 18, 24, 42),
            fontStyle = FontStyle.Bold,
            normal = { textColor = new Color(0.9f, 0.08f, 0.1f) }
        };
        promptStyle ??= new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = Mathf.Clamp(Screen.height / 35, 16, 24),
            normal = { textColor = Color.white }
        };

        GUI.Label(new Rect(panel.x + 20f, panel.y + 35f, panel.width - 40f, 90f), "YOU WERE CAUGHT\nGAME OVER", titleStyle);
        GUI.Label(new Rect(panel.x + 20f, panel.y + 145f, panel.width - 40f, 55f), "Press R to try again", promptStyle);
    }
}
