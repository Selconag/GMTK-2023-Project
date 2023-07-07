public class MergeableObject : Card
{
    public TMPro.TextMeshPro LevelText;

    private void Start()
    {
        LevelText.text = "Level " + ObjectLevel.ToString();
        if (Slot == null)
            StartPos = transform.position;
    }
}