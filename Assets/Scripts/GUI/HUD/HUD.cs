﻿using UnityEngine;
using System.Collections.Generic;

public class HUD : MonoBehaviour {

	private static HUD instance;

	public static string message;
	public static CustomTower TowerMessage;
	public static string Wave;
	public static bool gameOver = false;

	private Rect InfoCenterRect;

	public static bool CostStyle {
		set {
			if (value) {
				instance.costStyle = instance.AffordCostStyle;
			}
			else {
				instance.costStyle = instance.NoAffordCostStyle;
			}
		}
	}
	private GUIStyle costStyle;

	public GUIStyle RightAlignLabel;
	public GUIStyle GameOverStyle;

	public GUIStyle NoAffordCostStyle;
	public GUIStyle AffordCostStyle;

	// Use this for initialization
	void Start () {
		if (instance != null) {
			Destroy(instance);
		}
		instance = this;

		Vector2 botLeft = GUIUtilities.WorldToGuiScreen(new Vector3(-7.5f, 5, 0));
		Vector2 botRight = GUIUtilities.WorldToGuiScreen(new Vector3(7.5f, 5, 0));

		InfoCenterRect = this.CenteredRect(this.Width(.5f), botLeft.y / 2, botRight.x - botLeft.x, botLeft.y);
	}

	void OnLevelWasLoaded(int x) {
		gameOver = false;
		gameOverCountdown = 5;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI() {
		GUI.Box(InfoCenterRect, "");

		GUI.Label(this.AnchoredRect(
			GUIUtilities.RIGHT_MID,
			InfoCenterRect.width + InfoCenterRect.xMin - this.Width(.07f, InfoCenterRect.width),
			InfoCenterRect.height / 2,
			this.Width(.07f, InfoCenterRect.width),
			InfoCenterRect.height
			), "Money: ", RightAlignLabel);

		GUI.Label(this.AnchoredRect(
			GUIUtilities.RIGHT_MID,
			InfoCenterRect.width + InfoCenterRect.xMin,
			InfoCenterRect.height / 2,
			this.Width(.05f, InfoCenterRect.width),
			InfoCenterRect.height
			), "" + GameStats.Money, RightAlignLabel);

		if (TowerMessage != null) {
			ShowTowerMessage();
		}
		if (Wave != null) {
			ShowWaveMessage();
		}
		if (gameOver){
			ShowGameOver();
		}
	}

	private void ShowTowerMessage() {

		string message1 = "Cost:\nDamage:\nRange:";
		string message2 = "\nSpeed:";
		string cost = "" + TowerMessage.Cost;
		string stats1 = (Mathf.Round(TowerMessage.towerBase.GetComponent<TowerBase>().GetAttackDamage() * 100) / 100f) + "\n"+
			TowerMessage.towerBase.GetComponent<TowerBase>().range;
		string stats2 = "" + Mathf.Round(TowerMessage.towerBase.GetComponent<TowerBase>().GetAttackSpeed() * 100) / 100f;

		GUI.Label(this.AnchoredRect(
			GUIUtilities.LEFT_TOP,
			InfoCenterRect.xMin,
			InfoCenterRect.yMin,
			this.Width(.1f, InfoCenterRect.width),
			InfoCenterRect.height
			), message1);

		GUI.Label(this.AnchoredRect(
			GUIUtilities.LEFT_TOP,
			InfoCenterRect.xMin + 125,
			InfoCenterRect.yMin,
			this.Width(.1f, InfoCenterRect.width),
			InfoCenterRect.height
			), message2);

		GUI.Label(this.AnchoredRect(
			GUIUtilities.LEFT_TOP,
			InfoCenterRect.xMin,
			InfoCenterRect.yMin,
			this.Width(.1f, InfoCenterRect.width),
			InfoCenterRect.height
			), cost, costStyle);

		GUI.Label(this.AnchoredRect(
			GUIUtilities.LEFT_TOP,
			InfoCenterRect.xMin,
			InfoCenterRect.yMin + 18,
			this.Width(.1f, InfoCenterRect.width),
			InfoCenterRect.height
			), stats1, RightAlignLabel);

		GUI.Label(this.AnchoredRect(
			GUIUtilities.LEFT_TOP,
			InfoCenterRect.xMin + 125,
			InfoCenterRect.yMin + 18,
			this.Width(.1f, InfoCenterRect.width),
			InfoCenterRect.height
			), stats2, RightAlignLabel);
	}

	private void ShowWaveMessage() {
		GUI.Label (this.AnchoredRect(
			GUIUtilities.RIGHT_MID,
			InfoCenterRect.width + InfoCenterRect.xMin - this.Width(.07f, InfoCenterRect.width),
			InfoCenterRect.height / 2,
			this.Width(.07f, InfoCenterRect.width),
			InfoCenterRect.height
			), Wave, RightAlignLabel);
	}

	private float gameOverCountdown = 5;

	private void ShowGameOver(){
		gameOverCountdown-= Time.deltaTime;
		GUI.Label (this.AnchoredRect (
			GUIUtilities.MID_MID,
			this.Width(.5f),
			this.Height(.5f),
			this.Width(.5f), this.Height(.5f)),
			"Game Over", GameOverStyle);
		if(gameOverCountdown<=0) {
			Application.LoadLevel("Level Select");
		}

	}
}
