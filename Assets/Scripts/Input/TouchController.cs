﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(InputHandler),typeof(Grid))]
public class TouchController : MonoBehaviour {

	private InputHandler handler;
	private Grid grid;

	private Vector3 touchPosition;
	private Vector3 worldTouchPosition;

	public float DeadZone = 100f;
	public float CircleRadius = 2f;

	public bool placingTower = false;

	// Use this for initialization
	void Start () {
		grid = GetComponent<Grid>();
		handler = GetComponent<InputHandler>();
	}
	
	// Update is called once per frame
	void Update () {
		handler.handleInput();

		foreach(InputEvent e in handler.Events) {
			Vector3 worldPos = Utilities.ScreenToWorld(e.position);
			if (!placingTower && e.phase == TouchPhase.Began && grid.IsValid(grid.WorldToGridPosition(worldPos))) {
				touchPosition = e.position;
				worldTouchPosition = Utilities.ScreenToWorld(touchPosition);;
				
				ShowTowers();
				placingTower = true;
			}
			if (placingTower) {

				int idx = GetIndex(e.position);

				HUD.CostStyle = TowerSelectionMenu.Instance.towers[idx].Cost <= GameStats.Money;
				HUD.TowerMessage = TowerSelectionMenu.Instance.towers[idx];

				if (e.phase == TouchPhase.Ended) {
					if (Vector3.Distance(e.position, touchPosition) > DeadZone) {

						CustomTower tower = TowerSelectionMenu.Instance.towers[idx];
						if (GameStats.BuyTower(tower.Cost)) {
							bool placed = grid.PlaceTower(tower, touchPosition);
						}
					}

					HideTowers();
					placingTower = false;
					HUD.TowerMessage = null;
				}
			}
		}
	}

	void ShowTowers() {
		TowerSelectionMenu.Instance.bg.transform.position = worldTouchPosition;
		TowerSelectionMenu.Instance.bg.SetActive(true);
		for (int i = 0; i < 8; i++) {
			TowerSelectionMenu.Instance.towerInstances[i].transform.position = Circularize(i);
			TowerSelectionMenu.Instance.towerInstances[i].SetActive(true);
		}
	}

	void HideTowers() {
		TowerSelectionMenu.Instance.bg.SetActive(false);
		for (int i = 0; i < 8; i++) {
			TowerSelectionMenu.Instance.towerInstances[i].SetActive(false);
		}
	}

	int GetIndex(Vector3 release) {

		float rawAngle = Mathf.Atan2(release.y - touchPosition.y, release.x - touchPosition.x) - Mathf.PI / 2 - Mathf.PI / 8;
		rawAngle += rawAngle < 0 ? Mathf.PI * 2 : 0;

		return (int)(8f - rawAngle / (Mathf.PI * 2) * 8);
	}

	Vector3 Circularize(int index) {
		float angle = Mathf.PI / 2 + (8 - index) / 8.0f * Mathf.PI * 2;

		return new Vector3(
			Mathf.Cos(angle) * CircleRadius + worldTouchPosition.x,
			Mathf.Sin(angle) * CircleRadius + worldTouchPosition.y,
			0
		);
	}
}