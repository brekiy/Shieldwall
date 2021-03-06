﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class BaseGuiController : MonoBehaviour {
  public CellGrid CellGrid;
  public GameObject UnitsParent;
  public Button NextTurnButton;

  //public Image UnitImage;
  public Text InfoText;
  public Text StatsText;
  private GameObject StatsPanel;
  private GameObject AttackPanel;

  void Start() {
    //UnitImage.color = Color.gray;
    StatsPanel = GameObject.Find("Stats Panel");
    AttackPanel = GameObject.Find("Attack Panel");
    CellGrid.GameStarted += OnGameStarted;
    CellGrid.TurnEnded += OnTurnEnded;
    CellGrid.GameEnded += OnGameEnded;
  }

  private void OnGameStarted(object sender, EventArgs e) {
    foreach (Transform unit in UnitsParent.transform) {
      unit.GetComponent<Unit>().UnitHighlighted += OnUnitHighlighted;
      unit.GetComponent<Unit>().UnitDehighlighted += OnUnitDehighlighted;
      unit.GetComponent<Unit>().UnitAttacked += OnUnitAttacked;
    }
    foreach (Transform cell in CellGrid.transform) {
      cell.GetComponent<Cell>().CellHighlighted += OnCellHighlighted;
      cell.GetComponent<Cell>().CellDehighlighted += OnCellDehighlighted;
    }
    OnTurnEnded(sender, e);
  }

  private void OnGameEnded(object sender, EventArgs e) {
    InfoText.text = "Player " + ((sender as CellGrid).CurrentPlayerNumber + 1) + " wins!";
  }
  private void OnTurnEnded(object sender, EventArgs e) {
    NextTurnButton.interactable = ((sender as CellGrid).CurrentPlayer is HumanPlayer);
    InfoText.text = "Player " + ((sender as CellGrid).CurrentPlayerNumber + 1);
  }
  private void OnCellDehighlighted(object sender, EventArgs e) {
    //UnitImage.color = Color.gray;
    StatsPanel.SetActive(false);
    StatsText.text = "";
  }
  private void OnCellHighlighted(object sender, EventArgs e) {
    //UnitImage.color = Color.gray;
    StatsPanel.SetActive(true);
    StatsText.text = (sender as HexCell).Terrain + 
    "\nMovement Cost: " + (sender as Cell).MovementCost;
  }
  private void OnUnitAttacked(object sender, AttackEventArgs e) {
    if (!(CellGrid.CurrentPlayer is HumanPlayer)) return;
    OnUnitDehighlighted(sender, new EventArgs());
    if ((sender as Unit).HitPoints <= 0) return;
    OnUnitHighlighted(sender, e);
  }
  private void OnUnitDehighlighted(object sender, EventArgs e) {
    StatsPanel.SetActive(false);
    StatsText.text = "";
    //UnitImage.color = Color.gray;
  }
  private void OnUnitHighlighted(object sender, EventArgs e) {
    StatsPanel.SetActive(true);
    var unit = sender as FEUnit;
    StatsText.text = unit.UnitName + "\nHit Points: " + unit.HitPoints + "/" + unit.TotalHitPoints + "\nMelee Attack: "
    + unit.MeleeAttack + "\nRanged Attack: " + unit.RangedAttack + "\nMagic Attack: " + unit.MagicAttack +
    "\nRange: " + unit.RangedRange + "\nMagic Range: " + unit.MagicRange + "\nAmmunition: " + unit.Ammunition +
    "\nMelee Defense: " + unit.MeleeDefense + "\nArmor: " + unit.Armor + "\nResistance: " + unit.Resistance +
    "\nMorale: " + unit.Morale;
    //UnitImage.color = unit.PlayerColor;
  }

  public void RestartLevel() {
    Application.LoadLevel(Application.loadedLevel);
  }
}
