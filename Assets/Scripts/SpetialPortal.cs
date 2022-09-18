using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpetialPortal : Portal
{
    [SerializeField]
    protected Camera specialCamera;
    [SerializeField]
    string sBgm;


    protected override IEnumerator TransportTask(Transform _tr)
    {
        isTransporting = true;
        imgBlack.enabled = true;

        AudioController.instance.Mute();

        //畫面變暗
        float Proc = 0;
        while (imgBlack.color != Color.black)
        {
            imgBlack.color = Color.Lerp(Color.clear, Color.black, Proc += fLerpSpeed * Time.deltaTime);
            yield return null;
        }
        //Transport
        _tr.position = trDest.position;
        vcConfiner.m_BoundingShape2D = trDest.parent.Find("CameraConfiner").GetComponent<PolygonCollider2D>();
        Camera.main.enabled = false;
        specialCamera.enabled = true;


        yield return new WaitForSeconds(0.2f);
        //畫面恢復
        Proc = 0;
        while (imgBlack.color != Color.clear)
        {
            imgBlack.color = Color.Lerp(Color.black, Color.clear, Proc += fLerpSpeed * Time.deltaTime);
            yield return null;
        }
        AudioController.instance.PlayBgm(sBgm);
        AudioController.instance.Open();

        imgBlack.enabled = false;
        isTransporting = false;
    }
}
