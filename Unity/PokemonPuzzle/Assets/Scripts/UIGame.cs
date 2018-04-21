using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
	[SerializeField]
	private Text txtScore = null;

	[SerializeField]
	private Text txtChain = null;

	[SerializeField]
	private Text txtPauseTime = null;

	public void SetTextScore(int _newScore)
	{
		if(!txtScore) { return; }

		txtScore.text = "Score: " + _newScore.ToString("00000000");
	}

	public void SetTextChain(int _newChain)
	{
		if (!txtChain) { return; }

		txtChain.text = "Chain: " + _newChain.ToString();
	}

	public void SetTextPauseTimeRemaining(int _newTime)
	{
		if (!txtPauseTime) { return; }
	}
}
