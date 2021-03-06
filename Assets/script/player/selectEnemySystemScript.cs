using UnityEngine;
using System.Collections.Generic;
using myFunction;

public class selectEnemySystemScript : GameFunction
{
    playerContorl playercontorl;

    [HideInInspector]
    public GameObject[] GB; //目標所在的array
    [HideInInspector]
    public GameObject playerSelectPointerSystem; //取得selectPointerMovement程式 內容
    [SerializeField]
    public GameObject selectPointerUIpart;

    [SerializeField]
    [HideInInspector]
    gameStateDataClass gameStateDataClass;

    [SerializeField][HideInInspector]
    Transform pointer;  //以那個圖作?Ew 開始找最近的?EE
    [SerializeField][HideInInspector]
    pointerTrigger OnTriggerEnter2DCircle;
    [SerializeField]
    public float slowMotionTimeScale;
    [SerializeField]
    public float timeToComplete;
    [SerializeField]
    public int controlHpCost;


    GameObject[] TriggerArray;

    public GameObject targetGameObj;  //不是手動放進去

    GameObject lockDownTargetGO;

    Function myfunction = new Function();
    int selectTaget = 1;

    float[] eachEnemylerpFloat;

    int target = -1;
    float timerCount = 0.0f;

    public bool openTargetLockDown = false;

    // Use this for initialization
    void Start() {
        playercontorl = GetComponent<playerContorl>();
        TriggerArray = OnTriggerEnter2DCircle.TriggerList.ToArray();

        playerSelectPointerSystem.SetActive(false);
        selectPointerUIpart.SetActive(false);
        GB = getallenemyWithoutContorlOnce();
    }
    
    void startTargetLockDown(Vector3 centerObjectPosition, int targetNum) {
        #region �i: ?�S瞜,��?��?�S瞜���������W �o:targetGameObj
        GB = TriggerArray; //使用那個enemyArray?

        if (GB.Length > 0) { //TriggerArray內有東?E
            eachEnemylerpFloat = new float[GB.Length];  //化成每個敵人?EEw相差多少?
            short i = 0;
            foreach (GameObject each in GB) {
               // each.GetComponent<SpriteRenderer>().color = Color.white;
                eachEnemylerpFloat[i] = Vector3.Distance(centerObjectPosition, each.GetComponent<Transform>().position);
                i++;
            }

            target = myfunction.findSmallestOfBigestNumberInArray(eachEnemylerpFloat, false, targetNum);  //?�oeachEnemylerpFloat array?������������?

            targetGameObj = GB[target]; //把目標找出來
            //targetGameObj.GetComponent<SpriteRenderer>().color = Color.red;

            foreach (GameObject each in getallenemyWithoutContorlOnce()) {  //還?E狾貫nemy的顏?Ewithout targetgame  可行可用
                if (each.GetInstanceID() != targetGameObj.GetInstanceID() ) {
                    //each.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }

        }
        else {
            foreach (GameObject each in getallenemyWithoutContorlOnce()) {  //還?E狾貫nemy的顏?E
                //each.GetComponent<SpriteRenderer>().color = Color.white;

            }
        }


        //openTargetLockDown = true;
        #endregion
    }


    public void setupTargetLockDown() {  //
        transform.rotation = Quaternion.Euler(0,0,0);
        Time.timeScale = slowMotionTimeScale;  //湃淽整個遊戲速度
        //playercontorl.incontorlObj.GetComponent<playerMove>().enabled = false;
        targetGameObj = null;

        playerSelectPointerSystem.SetActive(true);  //開啟?E僆�E
        selectPointerUIpart.SetActive(true);
        GB = TriggerArray;
        openTargetLockDown = true;
    }

    public void cancelTargetLockDown() {  //取消function
        //GB = TriggerArray;
        Time.timeScale = 1f;
        //playercontorl.incontorlObj.GetComponent<playerMove>().enabled = true;
        playerSelectPointerSystem.SetActive(false);
        selectPointerUIpart.SetActive(false);
        openTargetLockDown = false;

        if (targetGameObj!=null) {
            //targetGameObj.GetComponent<SpriteRenderer>().color = Color.white;
        }

        target = -1;


    }

    // Update is called once per frame
    void Update() {

        if (gameStateDataClass.gamestate != gameStateDataClass.gameState.pause && playercontorl.incontorlObj) {
            if (Input.GetButtonDown("OpenCloseControlPreview")) {
                if (openTargetLockDown) {  //結?EE亃惆�E
                    cancelTargetLockDown();
                }
                else {  //開始?E亃惆�E
                    playerDataClass playerData = GameObject.FindGameObjectsWithTag("backgroundScipt")[0].GetComponent<playerDataClass>();
                    if (playerData.HP - controlHpCost > 0) { //�v���掊p���\屣��
                        playerData.HP -= controlHpCost;
                        setupTargetLockDown();
                        timerCount = 0;
                    }

                }
            }

        }


        if (openTargetLockDown) {
            TriggerArray = OnTriggerEnter2DCircle.TriggerList.ToArray();
            Vector3 pointerV3 = pointer.position;
            startTargetLockDown(pointerV3, 0);


            if (timerCount <= timeToComplete * slowMotionTimeScale ) { //?���v�Z�n? ���Z�islowMotion�I?��������
                timerCount += Time.deltaTime;
            }
            else { //����?���A��������
                cancelTargetLockDown();
            }

        }

        #region �E��code
        /*
        if (Input.GetKeyDown(KeyCode.LeftArrow) && openTargetLockDown) {
            if (selectTaget == 0) {
                selectTaget = eachEnemylerpFloat.Length - 1;
            }
            else {
                selectTaget--;
            }
            startTargetLockDown(getLeftestPointer.transform.position,selectTaget);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)&& openTargetLockDown) {



            if (eachEnemylerpFloat.Length-1 == selectTaget) {
                selectTaget = 0;
            }
            else {
                selectTaget++;
            }
            startTargetLockDown(getLeftestPointer.transform.position, selectTaget);
        }
        */

        //Debug.Log(selectTaget);

        /*
        if (Input.GetKeyUp(KeyCode.U)) {
            if (eachEnemylerpFloat.Length == selectTaget) {
                selectTaget = 0;
            }
            else {
                selectTaget++;
            }

        }
        */


        //Debug.Log(eachEnemylerpFloat[0]);




        //Debug.Log(myfunction.findSmallestOfBigestNumberInArray(testfloat, false,3));

        //開發一個左右鈕來?E僉鰷{在目標最近的另一隻怪物

        #endregion
    }
}
