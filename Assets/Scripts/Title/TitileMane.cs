using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitileMane : MonoBehaviour
{
    [SerializeField] AudioClip PlaySound;
    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void GamePlay()
    {
        audioSource.PlayOneShot(PlaySound);
        Initiate.Fade("GameScene", Color.black, 1.0f);
    }
    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }
}
