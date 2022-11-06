using System;
using System.Collections;
using System.Collections.Generic;
using cakeslice;
using Classes;
using UnityEngine;
using UnityEngine.InputSystem;

public class SquarePuzzleTile : MonoBehaviour, IInteractable
{
    [SerializeField] private SquarePuzzle parent;
    [SerializeField] public Material matBlack;
    [SerializeField] public Material matWhite;

    public bool on = false;
    private bool selected = false;
    private Outline outline;
    private MeshRenderer meshRenderer;
    private CameraLooking looking;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void init()
    {
        this.outline = GetComponent<Outline>();
        this.meshRenderer = GetComponent<MeshRenderer>();
        this.looking = GameObject.Find("PlayerCamera").GetComponent<CameraLooking>();
    }

    // Update is called once per frame
    void Update()
    {
        this.selected = looking.IsLooking(this.gameObject);
        this.outline.eraseRenderer = !selected;
    }

    private void Click()
    {
        parent.PieceClicked(this);
    }

    public void SilentClick()
    {
        on = !on;
        meshRenderer.material = on ? matBlack : matWhite;
    }

    public void Interact()
    {
        Click();
    }
}