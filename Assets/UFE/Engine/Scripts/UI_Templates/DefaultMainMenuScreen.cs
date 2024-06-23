using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DefaultMainMenuScreen : MainMenuScreen{
    #region public instance fields

    private RoundContorllerNew _controller;
    public AudioClip onLoadSound;
	public AudioClip music;
	public AudioClip selectSound;
	public AudioClip cancelSound;
	public AudioClip moveCursorSound;
	public bool stopPreviousSoundEffectsOnLoad = false;
	public float delayBeforePlayingMusic = 0.1f;

	public GameObject PatreonButton, DiscordButton;
	public Button buttonNetwork;
	public Button buttonBluetooth;
	public GameObject ButtonManager;
    #endregion

    private void Start()
    {
        if (PlayerPrefs.GetString("PSFW") == "Yes")
		{
            PatreonButton.gameObject.SetActive(false);
            DiscordButton.gameObject.SetActive(false);

        }
        else
		{
            PatreonButton.gameObject.SetActive(true);
            DiscordButton.gameObject.SetActive(true);

		}

        if (!PlayerPrefs.HasKey("SFW"))
			PlayerPrefs.SetString("SFW", "No");

		ButtonManager = GameObject.Find("ButtonManager");

		//UFE.config.inputOptions.inputManagerType = InputManagerType.UnityInputManager;
		_controller = Camera.main.GetComponent<RoundContorllerNew>();
		_controller.RoundStart = false;

		if (UFE.config.inputOptions.inputManagerType == InputManagerType.CustomClass)
            ButtonManager.SetActive(false);
    }

    #region public override methods
    public override void DoFixedUpdate(
		IDictionary<InputReferences, InputEvents> player1PreviousInputs,
		IDictionary<InputReferences, InputEvents> player1CurrentInputs,
		IDictionary<InputReferences, InputEvents> player2PreviousInputs,
		IDictionary<InputReferences, InputEvents> player2CurrentInputs
	){
		base.DoFixedUpdate(player1PreviousInputs, player1CurrentInputs, player2PreviousInputs, player2CurrentInputs);

		this.DefaultNavigationSystem(
			player1PreviousInputs,
			player1CurrentInputs,
			player2PreviousInputs,
			player2CurrentInputs,
			this.moveCursorSound,
			this.selectSound,
			this.cancelSound
		);
	}

	public override void OnShow (){
		base.OnShow ();
		this.HighlightOption(this.FindFirstSelectable());

		if (this.music != null){
			UFE.DelayLocalAction(delegate(){UFE.PlayMusic(this.music);}, this.delayBeforePlayingMusic);
		}
		
		if (this.stopPreviousSoundEffectsOnLoad){
			UFE.StopSounds();
		}
		
		if (this.onLoadSound != null){
			UFE.DelayLocalAction(delegate(){UFE.PlaySound(this.onLoadSound);}, this.delayBeforePlayingMusic);
		}

		if (buttonNetwork != null){
			buttonNetwork.interactable = UFE.isNetworkAddonInstalled;
		}

		if (buttonBluetooth != null){
            buttonBluetooth.interactable = UFE.isBluetoothAddonInstalled && Application.isMobilePlatform;
        }
	}
	#endregion
}
