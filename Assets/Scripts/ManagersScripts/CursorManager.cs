using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CursorType
{
    None,
    Arrow,
    HoverItem,
    ClickItem,
}
public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;


    [SerializeField] private List<CursorAnimation> cursorAnimationList;
    [SerializeField] private CursorAnimation cursorAnimation;
    [SerializeField] public CursorAnimation postCursorAnimation;
    [SerializeField] private int currentFrame;
    public float frameTimer;
    [SerializeField] int frameCount;

    public bool isLooping = false;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        //SetActiveCursorAnimation(CursorType.Arrow);
        PlayCursorAnimation(CursorType.Arrow);
        postCursorAnimation.cursorType = CursorType.None;
    }

    // Update is called once per frame
    void Update()
    {
        frameTimer -= Time.deltaTime;
    
            
            if (frameTimer <= 0f)
            {
                frameTimer += cursorAnimation.frameRate;
            
                currentFrame = (currentFrame + 1) % frameCount;
                Cursor.SetCursor(cursorAnimation.textureArray[currentFrame], cursorAnimation.offset, CursorMode.Auto);



            if (currentFrame >= frameCount - 1)
            {


                if (postCursorAnimation.cursorType != CursorType.None)
                {


                    SetActiveCursorAnimation(postCursorAnimation.cursorType);
                    postCursorAnimation.cursorType = CursorType.None;
                }
                else
                {
                    isLooping = false;
                }
            }


        }
        
      
        if (Input.GetKeyDown(KeyCode.T)) SetActiveCursorAnimation(CursorType.Arrow);
        if (Input.GetKeyDown(KeyCode.Y)) SetActiveCursorAnimation(CursorType.HoverItem);
        if (Input.GetKeyDown(KeyCode.U)) PlayCursorAnimation(CursorType.ClickItem, CursorType.Arrow);
    }

     public CursorAnimation GetCursorAnimation(CursorType p_cursorType)
     {
        foreach (CursorAnimation selectedCursorAnimation in cursorAnimationList)
        {

            if (selectedCursorAnimation.cursorType == p_cursorType)
            {
                return selectedCursorAnimation;
            }
        }
        Debug.Log("FOUND NOTHING");
        return null;
     }
    public void SetActiveCursorAnimation(CursorAnimation p_cursorAnimation)
    {
        if (p_cursorAnimation != null)
        {
            if (cursorAnimation.cursorType != p_cursorAnimation.cursorType)
            {

                cursorAnimation = p_cursorAnimation;

                currentFrame = 0;
                frameTimer = p_cursorAnimation.frameRate;
                frameCount = p_cursorAnimation.textureArray.Length;


            }
        }

    }

    public void SetActiveCursorAnimation(CursorType p_cursorType)
    {
        if (GetCursorAnimation(p_cursorType) != null)
        {
            if (cursorAnimation.cursorType != GetCursorAnimation(p_cursorType).cursorType)
            {

                cursorAnimation = GetCursorAnimation(p_cursorType);

                currentFrame = 0;
                frameTimer = cursorAnimation.frameRate;
                frameCount = cursorAnimation.textureArray.Length;


            }
        }
        


    }
    public void PlayCursorAnimation(CursorType p_cursorType, CursorType p_resetCursorType)
    {
        if (GetCursorAnimation(p_cursorType) != null)
        {
            if (cursorAnimation.cursorType != GetCursorAnimation(p_cursorType).cursorType)
            {
                isLooping = true;
                cursorAnimation = GetCursorAnimation(p_cursorType);
                currentFrame = 0;

                frameTimer = cursorAnimation.frameRate;
                frameCount = cursorAnimation.textureArray.Length;
                postCursorAnimation.cursorType = GetCursorAnimation(p_resetCursorType).cursorType;
            }
        }

        //cursorAnimation.cursorType = p_cursorAnimation.cursorType;
        //cursorAnimation.textureArray = p_cursorAnimation.textureArray;
        //cursorAnimation.frameRate = p_cursorAnimation.frameRate;
        //cursorAnimation.offset = p_cursorAnimation.offset;

    }
    public void PlayCursorAnimation(CursorType p_cursorType)
    {
        if (GetCursorAnimation(p_cursorType) != null)
        {
            
                isLooping = true;
            cursorAnimation.cursorType = GetCursorAnimation(p_cursorType).cursorType;
            cursorAnimation.textureArray = GetCursorAnimation(p_cursorType).textureArray;
            cursorAnimation.frameRate = GetCursorAnimation(p_cursorType).frameRate;
            cursorAnimation.offset = GetCursorAnimation(p_cursorType).offset;
           // cursorAnimation = GetCursorAnimation(p_cursorType);
            currentFrame = 0;

                frameTimer = cursorAnimation.frameRate;
                frameCount = cursorAnimation.textureArray.Length;
                postCursorAnimation.cursorType = GetCursorAnimation(p_cursorType).cursorType;
            
        }

        //cursorAnimation.cursorType = p_cursorAnimation.cursorType;
        //cursorAnimation.textureArray = p_cursorAnimation.textureArray;
        //cursorAnimation.frameRate = p_cursorAnimation.frameRate;
        //cursorAnimation.offset = p_cursorAnimation.offset;

    }
    [System.Serializable]
    public class CursorAnimation
    {
        public CursorType cursorType;
        public Texture2D[] textureArray;
        public float frameRate;
        public Vector2 offset;
    }
}
