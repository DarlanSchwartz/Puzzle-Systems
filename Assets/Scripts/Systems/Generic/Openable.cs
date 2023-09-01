using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Openable : SaveableObject
{
    public Animator anim;
    [Space(10)]
    public bool isOpen = false;
    public bool isLocked = false;
   
    public bool canLockIfOpen = false;
    public bool canBeOpenedWithKey = true;

    public bool requireKey = false;
    public int keyId;

    private int movingHash = Animator.StringToHash("Moving");
    private int openHash = Animator.StringToHash("Open");
    private int resetHash = Animator.StringToHash("Reset");

    public bool IsMoving
    {
        get
        {
            return anim.GetBool(movingHash);
        }
    }

    [Space(10)]
    [Header("Sounds")]
    [Space(10)]
    
    public AudioClip moveSound;
    public AudioClip closingSound;
    public AudioClip openingSound;
    public AudioClip lockedSound;
    public AudioClip lockSound;
    public AudioClip unlockSound;
    public AudioSource mainSource;
    public AudioSource secondarySource;

    private void OnDrawGizmos()
    {
        
    }

    public void Open()
    {
        if (anim.GetBool(movingHash) || isOpen)
            return;

        if (isLocked)
        {
            if (!mainSource.isPlaying)
            {
                mainSource.PlayOneShot(lockedSound);
            }

            return;
        }
            

        isOpen = true;
        anim.SetBool(openHash, true);
        anim.SetBool(movingHash, true);
        mainSource.PlayOneShot(moveSound);
    }

    public void Close()
    {
        if (anim.GetBool(movingHash) || !isOpen)
            return;

        if (isLocked && !isOpen)
        {
            if (!mainSource.isPlaying)
            {
                mainSource.PlayOneShot(lockedSound);
            }
            return;
        }
            
        isOpen = false;
        anim.SetBool(openHash, false);
        anim.SetBool(movingHash, true);
        mainSource.PlayOneShot(moveSound);
    }

    public void Interact()
    {
        if (anim.GetBool(movingHash))
            return;

        //Se ta trancada e fechada
        if (isLocked && !isOpen)
        {
            // Se precisa de chave checar se tem a chave no inventario do player, se tiver destrancar do contrario só tocar o som de trancada
            if (!requireKey)
            {
                if (!mainSource.isPlaying)
                {
                    mainSource.PlayOneShot(lockedSound);
                }
                return;
            }
            else
            {
                if(canBeOpenedWithKey)
                {
                    if (Inventory.instance.HasItem(keyId))
                    {
                        Unlock();
                        Inventory.instance.RemoveItem(keyId);
                        return;
                    }
                    else
                    {
                        if (!mainSource.isPlaying)
                        {
                            mainSource.PlayOneShot(lockedSound);
                        }
                        return;
                    }
                }
            }
        }

        // Se ta aberta fecha se ta fechada abre
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void Lock()
    {
        if(!isLocked)
        {
            if (!canLockIfOpen && isOpen)
                return;
            mainSource.PlayOneShot(lockSound);
            isLocked = true;
        }
    }

    public void Unlock()
    {
        if(isLocked)
        {
            mainSource.PlayOneShot(unlockSound);
            isLocked = false;
        }
    }

    public void SetLockState( string state)
    {
        if(state == "Locked")
        {
            Lock();
        }
        else if(state == "Unlocked")
        {
            Unlock();
        }
    }

    public void TryUnlockWithKey(int id)
    {
        if(requireKey && isLocked)
        {
            if(keyId == id)
            {
                Unlock();
            }
            else
            {
                mainSource.PlayOneShot(lockedSound);
            }
        }
    }

    public void ClosingSound()
    {
        if(secondarySource && closingSound)
        {
            secondarySource.PlayOneShot(closingSound,0.2f);
        }
    }

    public void OpeningSound()
    {
        if (secondarySource && openingSound)
        {
            secondarySource.PlayOneShot(openingSound, 0.2f);
        }
    }

    private void Awake()
    {
        mainSource = GetComponent<AudioSource>();
    }

    public override void Start()
    {
        // Add myself to the save game manager and set myTrans to this.transfrom in the base
        base.Start();
        anim.SetBool(movingHash, false);
        // Do this class code logic
    }

    public override void LoadFromCurrentData()
    {
        string[] loadedData = dataToSave.Split('|');

        isLocked = bool.Parse(loadedData[1]);

        bool open = bool.Parse(loadedData[0]);

        isOpen = open;
        anim.SetBool(openHash, open);
        anim.SetTrigger(resetHash);
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        dataToSave = isOpen.ToString() + "|" + isLocked.ToString();
    }

    public override void DestroySaveable()
    {
        // Destroy the saveable class and remove it from the saveable objects list
        base.DestroySaveable();
        // Actually Destroy this object
        Destroy(gameObject);
    }

    [ContextMenu("Get basic info")]
    private void GetBasicInfo()
    {
        anim = GetComponent<Animator>();

        AudioSource[] sources =  GetComponents<AudioSource>();

        mainSource = sources[0];
        secondarySource = sources[1];

#if UNITY_EDITOR
        moveSound = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/AudioClips/Props/Doors/Door_Move.aiff", typeof(AudioClip));
        closingSound = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/AudioClips/Props/Doors/Door_Closing.wav", typeof(AudioClip));
        //openingSound = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/AudioClips/Props/Doors/Door_Move.aiff", typeof(AudioClip));
        lockedSound = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/AudioClips/Props/Doors/Door_Locked.wav", typeof(AudioClip));
        lockSound = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/AudioClips/Props/Doors/Door_Lock.wav", typeof(AudioClip));
        unlockSound = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/AudioClips/Props/Doors/Door_Unlock.wav", typeof(AudioClip));
#endif
    }
}
