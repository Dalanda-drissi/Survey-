﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.UI;
using UnityEngine.Networking;
public class Vote : MonoBehaviour
{
        private List<Question> ListSurvey = new List<Question>();
        void Start(){
            ListSurvey=GetSurvey.ListSurvey;
        }
    private IEnumerator Answer()
   {
       Question question =new Question();
      
       List<Options> ListOptions = new List<Options>();
       string id="";
       var Label=this.gameObject.name;
       GameObject OptionContainer = GameObject.Find(Label+"/Content/Scroll View/Viewport/Content");
       foreach (var survey in ListSurvey)
       {
           if (survey.Label.Equals(Label)){
               id=survey._id;
              for (int i = 0; i < OptionContainer.transform.childCount; i++)
              {
                  if (OptionContainer.transform.GetChild(i).GetComponent<Toggle>().isOn==true)
                  {
                      foreach (var opt in survey.OptionsList)
                      {
                           Options option = new Options();
                          if (opt.Label.Equals(OptionContainer.transform.GetChild(i).name)){
                              var nb= int.Parse(opt.NBVote, System.Globalization.CultureInfo.InvariantCulture);
                              nb=nb+1;
                              option.Label=opt.Label;
                              option.OptionTxt=opt.OptionTxt;
                              option.NBVote=nb.ToString();
                              ListOptions.Add(option);
                          }
                          else{
                              option.Label=opt.Label;
                              option.OptionTxt=opt.OptionTxt;
                              option.NBVote=opt.NBVote;
                              ListOptions.Add(option);
                          }
                      }
                  }
              }
           }
       }
      
    question.OptionsList=ListOptions;
    
    var json = JsonUtility.ToJson(question);
    if (id!=""){
    UnityWebRequest putOptions = UnityWebRequest.Put("http://localhost:4000/Question/update/"+id,json);
    putOptions.SetRequestHeader("Content-Type", "application/json");
    putOptions.SetRequestHeader("Accept", "application/json");
    yield return putOptions.SendWebRequest();
    }
   else{
       Debug.Log("Vote invalide!");
   }
     

   }


   public void Voter(){
    StartCoroutine(Answer());
}
}

