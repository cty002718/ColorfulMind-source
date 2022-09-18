using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runPortal : Portal
{
	public Transform enemy;
    protected override IEnumerator TransportTask(Transform _tr)
    {
        isTransporting = true;

        //Transport
        _tr.position = trDest.position;

        // 主角暫停0.2秒
        yield return new WaitForSeconds(0.2f);

        isTransporting = false;

        yield return new WaitForSeconds(0.5f);

        enemy.position = trDest.position;

    }
}
