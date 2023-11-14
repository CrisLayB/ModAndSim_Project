using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Importar librerias para UI
using UnityEngine.UI;
using TMPro;

/*
 * Shuco:
 * Script que tendra todo el control sobre el manejo del
 * montaje, que se habilitara y que no estara habilitado.
 * El shuco esta conformado por los siguientes game objects:
 * - Pan Normal
 * ***********************************************************
 * - Pan Abierto
 * - Aguacate
 * - Repollo (Antes de cerrar)
 * - Salami
 * - Salchicha
 * - Tocino
 * - Chorizo
 * - Ketchup
 * - Mayonesa
 * - Mostasa
 * ***********************************************************
 * - Pan Listo
 * - Repollo (Despues de cerrar)
 * - Papel
 * - Bolsa
 * ***********************************************************
 * PD: Can be Cook estara marcado en false para evitar que
 *     empiece a cocerse.
 */
public class Shuco : Cook
{
    // Gameobjects que conformaran al Shuco
    [SerializeField] private GameObject breadClosed, breadOpen, breadReady; // *PANES*
    [SerializeField] private GameObject guacamole, cabbageBefore, cabbageAfter; // *AGUACATE Y REPOLLO*
    [SerializeField] private GameObject saugue, bakon, salami, chorizo; // *EMBUTIDOS*
    [SerializeField] private GameObject ketchup, mayonese, mustard; // *SALSAS*
    [SerializeField] private Spoon spoonState; // *LOGICA CUCHARA*
    [SerializeField] private GameObject tray, paper, plasticBag; // *PLATO, PAPEL y BOLSA DE PLASTICO*

    // States para detectar de una manera SEGURA la condicion del shuco
    private bool isBreadHot = false, isBreadOpen = false, isBreadReady = false;

    // ==> Elementos para el UI y el counter
    [SerializeField] private TextMesh informationText;    
    public CountController countController;
    private UIForShuco uiForShuco;

    // ==> Para tener el control de las salsas
    // No me gusta como esta estructurado esto de los segundos pero bueno jaja
    [SerializeField] private int saugueTime = 5;
    private int secondsKetchup = 0, secondsMayonese = 0, secondsMostard = 0;

    private AudioSource saugueSound;

    // ==> Tener el contro lde todos los elementos por medio de un hashmap/diccionario
    
    private Dictionary<int, bool> elementsDone = new Dictionary<int, bool>();
    

    // * =============================================================================
    // * === UNITY METHODS ===========================================================
    // * =============================================================================

    private void Awake() // Para encontrar los elementos escenciales para continuar
    {
        spoonState = FindObjectOfType<Spoon>();
        informationText = FindObjectOfType<TextMesh>();
        countController = FindObjectOfType<CountController>();
        uiForShuco = FindObjectOfType<UIForShuco>();

        GameObject gameObjectSaugeSound = GameObject.Find("SaugueSound");

        if(gameObjectSaugeSound != null)
        {
            saugueSound = gameObjectSaugeSound.GetComponent<AudioSource>();

            if(saugueSound == null) Debug.LogError("Saugue Sound Audio Source is Null");
        }
        else Debug.LogError("Saugue Sound Game Object is Null");

        if(uiForShuco == null)
        {
            Debug.LogError("UI For Shuco Selected is Null");
        }

        for(int i = 0; i < uiForShuco.TotalElements; i++)
        {
            elementsDone.Add(i, false);
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        // PASO 1) Vamos a abrir el pan (Si se hace contacto con el cuchillo)
        if (other.gameObject.tag.Equals("knife") && isBreadOpen == false)
        {
            AudioManager.Play(AudioClipName.knifesharpener1);
            isBreadOpen = true;            
            breadClosed.SetActive(false);
            breadOpen.SetActive(true);
            ChangueCookToTrue();
        }

        // PASO 2) Vamos a calentar el pan
        StartToCook(other); // other = objeto que esta colisionando

        // PASO 3) Vamos a ingresar todos los ingredientes para el shuco
        if (isBreadOpen == true && isBreadHot == true)
        {
            ActivateAndDestroyElementOfShuco("saugueDone", other, saugue, AudioClipName.pop2, 5);
            ActivateAndDestroyElementOfShuco("bakonDone", other, bakon, AudioClipName.pop2, 6);
            ActivateAndDestroyElementOfShuco("salamiDone", other, salami, AudioClipName.pop2, 7);
            ActivateAndDestroyElementOfShuco("chorizoDone", other, chorizo, AudioClipName.pop2, 8);

            if(other.gameObject.tag.Equals("spoon"))
            {
                // Agregar aguacate
                if(spoonState.Contents == 1)
                {
                    guacamole.SetActive(true);
                    AudioManager.Play(AudioClipName.guacamole_sound);
                    elementsDone[0] = true;
                    uiForShuco.ChangeStateShuco(elementsDone);
                }
                // Agregar repollo
                if(spoonState.Contents == 2)
                {
                    cabbageBefore.SetActive(true);
                    AudioManager.Play(AudioClipName.cabague_sound);
                    elementsDone[1] = true;
                    uiForShuco.ChangeStateShuco(elementsDone);
                }
            }
        }        

        // PASO 6) Cuando el shuco ya este cerrado se procedera a ingresar el plato, bolsa y papel
        if(isBreadReady)
        {
            ActivateAndDestroyElementOfShuco("tray", other, tray, AudioClipName.plastic_bag_sound); // Ingresar Plato
            ActivateAndDestroyElementOfShuco("plasticBag", other, plasticBag, AudioClipName.plastic_bag_sound); // Ingresar bolsa
            ActivateAndDestroyElementOfShuco("paper", other, paper, AudioClipName.plastic_bag_sound); // Ingresar papel
        }

        // PASO 7) Sellar el shuco con la parrilla (para sumar punteo)
        if (other.gameObject.tag.Equals("barbecue") && isShucoComplete())
        {
            AudioManager.Play(AudioClipName.bell);
            countController.addCount();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // PASO 4) Vamos a agregar las salsas correspondientes (Ketchup, Mayonesa y Mostaza)
        if (isBreadOpen == true && isBreadHot == true)
        {
            if(other.gameObject.CompareTag("ketchup"))
            {
                AudioManager.PlayContinueSound(saugueSound, true);
                StartCoroutine("AddKetchup");
            }

            if(other.gameObject.CompareTag("mayonese"))
            {
                AudioManager.PlayContinueSound(saugueSound, true);
                StartCoroutine("AddMayonese");
            }

            if(other.gameObject.CompareTag("mustard"))
            {
                AudioManager.PlayContinueSound(saugueSound, true);
                StartCoroutine("AddMustard");
            }
        }
        
        // ! ESTE SE DEBERA DE CAMBIAR POR UN METODO PARA EL MOUSE
        if(other.gameObject.tag.Equals("hitBoxHand")) // Detectar si las manos estan cerca del shuco
        {
            // PASO 5) Si el shuco tiene los ingredientes completos entonces se procedera a cerrar
            if(isShucoIngredientsComplete()) // Si dado caso el shuco ya tiene montado todos los ingredientes esperados
            {
                informationText.text = "Presiona click derecho para cerrar el shuco"; // Cuando la mano este en el pan nostros podremos cerrar el shuco
                // Ahora vamos a esperar el input del boton A para cambiar el modelo del shuco
                // if(OVRInput.GetDown(buttonOneforVR, OVRInput.Controller.RTouch) || OVRInput.GetDown(buttonOneforVR, OVRInput.Controller.LTouch))
                // {
                //     // Esto significa que el shuco ya se termino de cocinar de una forma exitosa
                //     informationText.text = "";
                //     breadOpen.SetActive(false);
                //     cabbageBefore.SetActive(false);
                //     breadReady.SetActive(true);
                //     cabbageAfter.SetActive(true);
                //     isBreadReady = true;
                //     AudioManager.Play(AudioClipName.pop3);
                // }
            }

            // PASO 7) Solo se indicara que el shuco ya esta completo cuando una de las manos del jugador este cerca del shuco
            if(isShucoComplete())
            {
                informationText.text = "Shuco Listo, mandalo a la parilla";
            }

            // EXTRA: Mostrar el estado del shuco que se esta preparando
            if(!isBreadHot)
            {
                uiForShuco.ActiveTextSelectedShuco(true);
            }

            if(isBreadHot)
            {
                uiForShuco.ActiveShucoElementsSelected(true);
                uiForShuco.ChangeStateShuco(elementsDone);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        informationText.text = "";
        AudioManager.PlayContinueSound(saugueSound, false);
        StopCoroutine("AddKetchup");
        StopCoroutine("AddMayonese");
        StopCoroutine("AddMustard");
        uiForShuco.ActiveTextSelectedShuco(false);
        uiForShuco.ActiveShucoElementsSelected(false);
    }

    // * =============================================================================
    // * === OWN METHODS =============================================================
    // * =============================================================================

    private void ActivateAndDestroyElementOfShuco(string tagElement, Collision other, GameObject obElement, AudioClipName audioClipName)
    {
        if(other.gameObject.tag.Equals(tagElement) && !obElement.activeSelf)
        {
            AudioManager.Play(audioClipName);
            obElement.SetActive(true);
            GameObject gameObjectTemp = other.gameObject;
            gameObjectTemp.transform.position = new Vector3(-1.3f, -0.50f, -5f);
        }
    }

    private void ActivateAndDestroyElementOfShuco(string tagElement, Collision other, GameObject obElement, AudioClipName audioClipName, int numElement)
    {
        if(other.gameObject.tag.Equals(tagElement) && !obElement.activeSelf)
        { 
            AudioManager.Play(audioClipName);
            obElement.SetActive(true);
            GameObject gameObjectTemp = other.gameObject;
            gameObjectTemp.transform.position = new Vector3(-1.3f, -0.50f, -5f);
            elementsDone[numElement] = true;
            uiForShuco.ChangeStateShuco(elementsDone);
        }
    }
    
    override public void ChangueMaterial()
    {
        AudioManager.Play(AudioClipName.point_bell);
        breadOpen.GetComponent<MeshRenderer>().material = materialReady;
        isBreadHot = true;
    }

    // Vamos a detectar si el shuco tiene todos los ingredientes y condiciones para denominarlo como completo
    private bool isShucoIngredientsComplete(){        
        return (breadOpen.activeSelf && saugue.activeSelf && bakon.activeSelf &&
                salami.activeSelf && chorizo.activeSelf &&
                guacamole.activeSelf && cabbageBefore.activeSelf &&
                ketchup.activeSelf && mayonese.activeSelf && mustard.activeSelf);
    }

    // Si el plato, bolsa de plastico y el papel estan activados indicaremos que el shuco ya esta completo
    public bool isShucoComplete(){
        return (tray.activeSelf && paper.activeSelf && plasticBag.activeSelf);
    }

    IEnumerator AddKetchup()
    {
        while(secondsKetchup <= saugueTime)
        {
            informationText.text = "ketchup: " + secondsKetchup + " s / " + saugueTime + " s";
            yield return new WaitForSeconds(1f);
            secondsKetchup++;
            if(secondsKetchup >= saugueTime)
            {
                ketchup.SetActive(true);
                elementsDone[2] = true;
                uiForShuco.ChangeStateShuco(elementsDone);
            }
        }
    }

    IEnumerator AddMayonese()
    {
        while(secondsMayonese <= saugueTime)
        {
            informationText.text = "Mayonese: " + secondsMayonese + " s / " + saugueTime + " s";
            yield return new WaitForSeconds(1f);
            secondsMayonese++;
            if(secondsMayonese >= saugueTime)
            {
                mayonese.SetActive(true);
                elementsDone[3] = true;
                uiForShuco.ChangeStateShuco(elementsDone);
            }
        }
    }

    IEnumerator AddMustard()
    {
        while(secondsMostard <= saugueTime)
        {
            informationText.text = "Mustard: " + secondsMostard + " s / " + saugueTime + " s";
            yield return new WaitForSeconds(1f);
            secondsMostard++;
            if(secondsMostard >= saugueTime)
            {
                mustard.SetActive(true);
                elementsDone[4] = true;
                uiForShuco.ChangeStateShuco(elementsDone);
            }
        }
    }

    // Pido perdón por repetir mucho código :(
}
