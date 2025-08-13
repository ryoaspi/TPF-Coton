using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class scrpt_spawnCard : MonoBehaviour
{

	public GameObject[] m_cards;
	[Space(5)]
	public TMP_Text m_nameText;
	public TMP_Text m_artNameText;
	public Volume m_postProcessVolume;

	//Private
	int currentIndex = 0;
	GameObject currentObject;
	scrpt_cardProfile _cardProfile;
	GameObject _spawner;


	void Start()
	{
		_spawner = this.gameObject;
		currentObject = Instantiate(m_cards[currentIndex]);
		currentObject.transform.SetParent(_spawner.transform);

		GetCard();
	}

	public void Update()
	{
		currentObject.transform.rotation = _spawner.transform.rotation;
	}

	public void Previous()
	{
		Destroy(currentObject);
		currentIndex--;
		if (currentIndex > m_cards.Length || currentIndex < 0)
		{
			currentIndex = m_cards.Length - 1;
		}

		currentObject = Instantiate(m_cards[currentIndex]);
		currentObject.transform.SetParent(_spawner.transform);

		GetCard();
	}

	public void Next()
	{
		Destroy(currentObject);
		currentIndex++;
		if (currentIndex > (m_cards.Length - 1) || currentIndex < 0)
		{
			currentIndex = 0;
		}
		currentObject = Instantiate(m_cards[currentIndex]);
		currentObject.transform.SetParent(_spawner.transform);

		GetCard();
	}

	public void GetCard()
	{
		_cardProfile = currentObject.GetComponent<scrpt_cardProfile>();
		m_nameText.text = _cardProfile.m_name;
		m_artNameText.text = _cardProfile.m_artistname;
		Camera.main.backgroundColor = _cardProfile.m_background;
		m_postProcessVolume.profile = _cardProfile.m_profile;
		print(m_postProcessVolume.profile);
	}
}
