using NUnit.Framework.Interfaces;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private float footstepTimer;
    private float footstepTimerMax = .4f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if(footstepTimer < 0f)
        {
            footstepTimer = footstepTimerMax;
            if(player.IsWalking()) SoundManager.Instance.PlayFootstepsSound(player.transform.position);
        }
    }
}
