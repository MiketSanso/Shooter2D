using UnityEngine;
using Zenject;

public class AnimController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private PrefabsManager _prefabsManager;
    [SerializeField] private InputController inputController;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] footstepsBare;
    [SerializeField] private AudioClip jumpSound;

    private AudioClip[] stepsActiveAudioClips;

    [HideInInspector] public GameObject lastGun, nextGun;

    [Inject]
    public void Construct(PrefabsManager prefabsManager)
    {
        _prefabsManager = prefabsManager;
    }

    public void MoveGunUpEnded()
    {
        animator.ResetTrigger("upGun");
        PlayerPrefs.SetInt("StopAllAnimations", 0);
    }

    public void MoveGunDownEnded()
    {
        if (inputController.lastGunID == "arm")
            animator.ResetTrigger("downHands");
        else
            animator.ResetTrigger("downGun");


        if (PlayerPrefs.GetString("idActiveGun") == "arm")
        {
            animator.SetTrigger("upHands");

            for (int b = 0; b < _prefabsManager.gunsUsable.Length; b++)
                _prefabsManager.gunsUsable[b].SetActive(false);
            return;
        }
        else
        {
            animator.SetTrigger("upGun");

            for (int a = 0; a < _prefabsManager.gunsUsable.Length; a++)
            {
                if (_prefabsManager.gunsUsable[a].GetComponent<Item>().id == PlayerPrefs.GetString("idActiveGun"))
                    _prefabsManager.gunsUsable[a].SetActive(true);
                else
                    _prefabsManager.gunsUsable[a].SetActive(false);
            }
        }
    }

    public void MoveHandsEnded()
    {
        animator.ResetTrigger("upHands");
        PlayerPrefs.SetInt("StopAllAnimations", 0);
    }

    public void JumpEnded()
    {
        animator.ResetTrigger("jump");
        audioSource.PlayOneShot(jumpSound);
    }

    public void TwoLightBlowEnded()
    {
        animator.ResetTrigger("twoLightBlow");
        PlayerPrefs.SetInt("StopAllAnimations", 0);
        PlayerPrefs.Save();
    }

    public void TwoHeavyBlowEnded()
    {
        animator.ResetTrigger("twoHeavyBlow");
        PlayerPrefs.SetInt("StopAllAnimations", 0);
        PlayerPrefs.Save();
    }

    public void StepSound()
    {
        stepsActiveAudioClips = footstepsBare; //FixWithInventory
        if (!PlayerPrefs.HasKey("StepNumber"))
        {
            PlayerPrefs.SetInt("StepNumber", 0);
            audioSource.PlayOneShot(stepsActiveAudioClips[0]);
        }
        else
        {
            int stepNumber = Random.Range(0, stepsActiveAudioClips.Length);

            if (stepNumber == PlayerPrefs.GetInt("StepNumber"))
            {
                stepNumber = stepsActiveAudioClips.Length - 1;
                PlayerPrefs.SetInt("StepNumber", stepsActiveAudioClips.Length - 2);
            }

            PlayerPrefs.SetInt("StepNumber", stepNumber);
            PlayerPrefs.Save();
            audioSource.PlayOneShot(stepsActiveAudioClips[stepNumber]);
        }
    }
}
