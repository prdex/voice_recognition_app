using UnityEngine;
using System.Collections;
using System.IO.Ports;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;
using System.Linq;

public class voice : MonoBehaviour {

	public SerialPort sp;
	public string port = "COM5";                //communication port
	public int speed = 115200; 				//speed of the communication (baud)

	//open bluetooth communication 

	KeywordRecognizer keywordRecognizer;
	Dictionary<string,System.Action> keywords = new Dictionary<string,System.Action> ();

	void Start(){

		keywords.Add ("Door open", () => {

			GoCalled();
		});
		keywordRecognizer = new KeywordRecognizer (keywords.Keys.ToArray ());
		keywordRecognizer.OnPhraseRecognized += keywordRecognizeronOnPhraseRecognized;
		keywordRecognizer.Start ();


	}

	void keywordRecognizeronOnPhraseRecognized(PhraseRecognizedEventArgs args){
		System.Action keywordaction;
		if (keywords.TryGetValue (args.text, out keywordaction)) {
			keywordaction.Invoke ();
			
		}
	
	}

	void GoCalled(){
		print ("door open");
	}

	public void openConnection() 
	{
		if(sp == null)
		{
			sp = new SerialPort(port, speed, Parity.None, 8, StopBits.One);
		}
		if (sp != null) 
		{
			if (!sp.IsOpen)
			{
				try
				{
					sp.Open();  			// opens the connection
					sp.ReadTimeout = 100;  // sets the timeout value before reporting error
					Debug.Log("Port Open!");
				}catch(System.Exception e)
				{
					Debug.LogWarning(e.ToString());
				}
			}
		}
	}
	//close connexion
	public void closeConnection() 
	{
		if(sp != null && sp.IsOpen)
		{
			try
			{
				sp.Close();
			}
			catch(System.Exception e)
			{
				Debug.LogWarning(e.ToString());
			}
		}
	}

}
