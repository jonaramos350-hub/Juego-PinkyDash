using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CambioEscenaVideo : MonoBehaviour
{
    private VideoPlayer video;
    public int proximoIndice;

    void Start()
    {
        video = GetComponent<VideoPlayer>();
        if (video != null)
        {
            video.loopPointReached += AlTerminarVideo;
            video.errorReceived += (v, msg) => IrAPantallaCarga();
        }
    }

    void AlTerminarVideo(VideoPlayer vp) { IrAPantallaCarga(); }
    void Update() { if (Input.GetKeyDown(KeyCode.Space)) IrAPantallaCarga(); }

    void IrAPantallaCarga()
    {
        ControladorFlujo.indiceObjetivo = proximoIndice;
        SceneManager.LoadScene(1);
    }
}