#pragma strict

var image1: UI.Image;
var image2: UI.Image;
var image3: UI.Image;

var fadeTime: float;

var timer: int;

function Start () 
{
  
}

function Update () 
{
    if(timer > 300)
    {
        image1.CrossFadeAlpha(0, fadeTime, true);
        if(timer > 600)
        {
            image2.CrossFadeAlpha(0, fadeTime, true);
            if(timer > 1200)
            {
                image3.CrossFadeAlpha(0, fadeTime, false);
                if(timer > 2400)
                {
                    //Application.LoadLevel("LEVEL_NAME");
                }
            }
        }
    }
    timer++;
}