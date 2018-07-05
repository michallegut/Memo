using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {
    public Button card11;
    public Button card12;
    public Button card13;
    public Button card14;
    public Button card21;
    public Button card22;
    public Button card23;
    public Button card24;
    public Button card31;
    public Button card32;
    public Button card33;
    public Button card34;
    public Button card41;
    public Button card42;
    public Button card43;
    public Button card44;
    public EventSystem eventSystem;
    public GameObject finishPanel;
    public Sprite bar;
    public Sprite circle;
    public Sprite elipse;
    public Sprite moon;
    public Sprite plus;
    public Sprite rectangle;
	public Sprite reverse;
    public Sprite ring;
    public Sprite triangle;
    public Text scoreText;

	private Button[,] cards;
    private List<Sprite> obversesPool;
    private Sprite[,] obverses;
    private int score;
	private Button lastRevaled;

    void Start() {
		cards = new Button[,]
        {
			{ card11, card12, card13, card14 },
			{ card21, card22, card23, card24 },
			{ card31, card32, card33, card34 },
			{ card41, card42, card43, card44 }
		};
		prepareGame();
	}

    public void replayButtonOnClick()
    {
		foreach (Button card in cards)
        {
			card.image.sprite = reverse;
		}
		prepareGame();
    }

    public void exitButtonOnClick()
    {
        Application.Quit();
    }

	public void cardOnClick(Button card)
    {
		if (card.image.sprite == reverse)
		{
			card.image.sprite = obverses [card.name.ElementAt (4) - '1', card.name.ElementAt (5) - '1'];
			if (lastRevaled == null)
			{
				lastRevaled = card;
			}
			else {
				if (card.image.sprite == lastRevaled.image.sprite)
                {
					score += 10;
                    lastRevaled = null;
                }
				else
				{
					score -= 2;
                    eventSystem.gameObject.SetActive(false);
					StartCoroutine(waitOneSecondAndReverse(card));
                }
				scoreText.text = "Score: " + score;
			}
		}
        if (isFinished())
        {
            finishPanel.SetActive(true);
        }
    }

    private void prepareGame()
    {
        finishPanel.SetActive(false);
		scoreText.text = "Score: 0";
        obversesPool = new List<Sprite>();
        for (int i = 0; i < 2; i++)
        {
            obversesPool.Add(bar);
            obversesPool.Add(circle);
            obversesPool.Add(elipse);
            obversesPool.Add(moon);
            obversesPool.Add(plus);
            obversesPool.Add(rectangle);
            obversesPool.Add(ring);
            obversesPool.Add(triangle);
        }
        obversesPool = obversesPool.OrderBy(x => Random.value).ToList();
        obverses = new Sprite[4, 4];
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				obverses[i, j] = obversesPool.First();
				obversesPool.RemoveAt(0);
			}
		}
		score = 0;
		lastRevaled = null;
    }

	private IEnumerator waitOneSecondAndReverse(Button card)
	{
		yield return new WaitForSeconds(1);
        eventSystem.gameObject.SetActive(true);
        card.image.sprite = reverse;
        lastRevaled.image.sprite = reverse;
        lastRevaled = null;
    }

    private bool isFinished()
    {
        bool result = true;
        for (int i = 0; result && i < 4; i++)
        {
            for (int j = 0; result && j < 4; j++)
            {
                if (cards[i, j].image.sprite == reverse)
                {
                    result = false;
                }
            }
        }
        return result;
    }
}