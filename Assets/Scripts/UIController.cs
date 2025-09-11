using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum Genders
{
	None,
	Male,
	Female
}

public enum Moods
{
	None,
	Depression,
	Burnout
}

public class UIController : MonoBehaviour
{
	[Header("FilterMenuButton")]
	public Button Button_FilterMenu;
	public GameObject FilterMenu;
	bool _filterToggle = false;

	[Space]
	[Header("AgeFilter")]
	public Toggle Toggle_Age;
	public Slider Slider_Age;
	public TextMeshProUGUI ValueText_Age;

	[Space]
	[Header("WeightFilter")]
	public Toggle Toggle_Weight;
	public Slider Slider_Weight;
	public TextMeshProUGUI ValueText_Weight;

	[Space]
	[Header("HeightFilter")]
	public Toggle Toggle_Height;
	public Slider Slider_Height;
	public TextMeshProUGUI ValueText_Height;

	[Space]
	[Header("AlcoholFilter")]
	public Toggle Toggle_Alcohol;
	public Slider Slider_Alcohol;
	public TextMeshProUGUI ValueText_Alcohol;

	[Space]
	[Header("GenderFilter")]
	public TMP_Dropdown Dropdown_Gender;

	[Space]
	[Header("MoodFilter")]
	public TMP_Dropdown Dropdown_Mood;

	[Space]
	public Transform LeftJoystick;

	private void Start()
	{
		SetupGenderDropdown();
		SetupMoodDropdown();
	}

	//Adding all functions for the UI by script
	private void OnEnable()
	{
		if (Button_FilterMenu != null) Button_FilterMenu.onClick.AddListener(OnFilterMenuButton);

		if (Toggle_Age != null) Toggle_Age.onValueChanged.AddListener(OnAgeToggle);
		if (Toggle_Weight != null) Toggle_Weight.onValueChanged.AddListener(OnWeightToggle);
		if (Toggle_Height != null) Toggle_Height.onValueChanged.AddListener(OnHeightToggle);
		if (Toggle_Alcohol != null) Toggle_Alcohol.onValueChanged.AddListener(OnAlcoholToggle);

		if (Slider_Age != null) Slider_Age.onValueChanged.AddListener(OnAgeSlider);
		if (Slider_Weight != null) Slider_Weight.onValueChanged.AddListener(OnWeightSlider);
		if (Slider_Height != null) Slider_Height.onValueChanged.AddListener(OnHeightSlider);
		if (Slider_Alcohol != null) Slider_Alcohol.onValueChanged.AddListener(OnAlcoholSlider);

		if (Dropdown_Gender != null) Dropdown_Gender.onValueChanged.AddListener(OnGenderChanged);
		if (Dropdown_Mood != null) Dropdown_Mood.onValueChanged.AddListener(OnMoodChanged);
	}

	private void OnDisable()
	{
		if (Button_FilterMenu != null) Button_FilterMenu.onClick.RemoveAllListeners();

		if (Toggle_Age != null) Toggle_Age.onValueChanged.RemoveAllListeners();
		if (Toggle_Weight != null) Toggle_Weight.onValueChanged.RemoveAllListeners();
		if (Toggle_Height != null) Toggle_Height.onValueChanged.RemoveAllListeners();

		if (Slider_Age != null) Slider_Age.onValueChanged.RemoveAllListeners();
		if (Slider_Weight != null) Slider_Weight.onValueChanged.RemoveAllListeners();
		if (Slider_Height != null) Slider_Height.onValueChanged.RemoveAllListeners();
		if (Slider_Alcohol != null) Slider_Alcohol.onValueChanged.RemoveAllListeners();

		if (Dropdown_Gender != null) Dropdown_Gender.onValueChanged.RemoveAllListeners();
		if (Dropdown_Mood != null) Dropdown_Mood.onValueChanged.RemoveAllListeners();
	}

	//Open and close filter menu
	void OnFilterMenuButton()
	{
		_filterToggle = !_filterToggle;
		FilterMenu.SetActive(_filterToggle);
		LeftJoystick.position = _filterToggle ? new Vector3(1060, LeftJoystick.position.y, LeftJoystick.position.z) : new Vector3(256, LeftJoystick.position.y, LeftJoystick.position.z);
	}

	//Filter for gender
	void SetupGenderDropdown()
	{
		if (Dropdown_Gender == null) return;

		Dropdown_Gender.ClearOptions();

		List<string> options = new List<string>();
		foreach (var gender in System.Enum.GetValues(typeof(Genders)))
		{
			options.Add(gender.ToString());
		}

		Dropdown_Gender.AddOptions(options);
		Dropdown_Gender.value = 0;
		Dropdown_Gender.RefreshShownValue();
	}

	void OnGenderChanged(int index)
	{
		Genders selectedGender = (Genders)index;

		switch (selectedGender)
		{
			case Genders.None:
				print("Selected: None");
				break;
			case Genders.Male:
				print("Selected: Male");
				break;
			case Genders.Female:
				print("Selected: Female");
				break;
		}
	}

	//Filter for moods
	void SetupMoodDropdown()
	{
		if (Dropdown_Mood == null) return;

		Dropdown_Mood.ClearOptions();

		List<string> options = new List<string>();
		foreach (var mood in System.Enum.GetValues(typeof(Moods)))
		{
			options.Add(mood.ToString());
		}

		Dropdown_Mood.AddOptions(options);
		Dropdown_Mood.value = 0;
		Dropdown_Mood.RefreshShownValue();
	}

	void OnMoodChanged(int index)
	{
		Moods selectedMood = (Moods)index;

		switch (selectedMood)
		{
			case Moods.None:
				print("Selected: None");
				break;
			case Moods.Depression:
				print("Selected: Depression");
				break;
			case Moods.Burnout:
				print("Selected: Burnout");
				break;
		}
	}

	//All toggle and slider filters
	void OnAgeToggle(bool isOn) => Slider_Age.interactable = isOn;
	void OnAgeSlider(float value) => ValueText_Age.text = value.ToString();

	void OnWeightToggle(bool isOn) => Slider_Weight.interactable = isOn;
	void OnWeightSlider(float value) => ValueText_Weight.text = value + "kg".ToString();

	void OnHeightToggle(bool isOn) => Slider_Height.interactable = isOn;
	void OnHeightSlider(float value) => ValueText_Height.text = value + "cm".ToString();

	void OnAlcoholToggle(bool isOn) => Slider_Alcohol.interactable = isOn;
	void OnAlcoholSlider(float value) => ValueText_Alcohol.text = value + "g".ToString();
}
