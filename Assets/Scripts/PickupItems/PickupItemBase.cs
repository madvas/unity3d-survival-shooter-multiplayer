﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class PickupItemBase : Photon.MonoBehaviour
{

	bool SentPickup;

	void OnTriggerEnter (Collider other)
	{
		PhotonView otherpv = other.GetComponent<PhotonView> ();
		if (otherpv != null && otherpv.isMine) {
			Debug.Log ("OnTriggerEnter() calls Pickup().");
			Pickup (otherpv.viewID);
		}
	}


	void Pickup (int playerPhotonViewId)
	{
		if (SentPickup) {
			return;
		}
		
		SentPickup = true;
		photonView.RPC ("PunPickup", PhotonTargets.AllViaServer, playerPhotonViewId);
	}


	[PunRPC]
	public void PunPickup (int playerPhotonViewId, PhotonMessageInfo msgInfo)
	{
		GameObject player = GameObjectHelper.FindPlayerByPhotonViewId (playerPhotonViewId);
		gameObject.SendMessage ("OnActivateItemEffect", player);
		if (msgInfo.sender.isLocal) {
			SentPickup = false;
			PhotonNetwork.Destroy (gameObject);
		}
	}
}
