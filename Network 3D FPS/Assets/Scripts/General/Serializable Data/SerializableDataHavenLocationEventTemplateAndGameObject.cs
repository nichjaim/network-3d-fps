using UnityEngine;

[System.Serializable]
public class SerializableDataHavenLocationEventTemplateAndGameObject
{
    #region Class Variables

    public HavenLocationEventTemplate valueTemplate;
    public GameObject valueObject;

    #endregion




    #region Constructors

    public SerializableDataHavenLocationEventTemplateAndGameObject()
    {
        Setup();
    }

    public SerializableDataHavenLocationEventTemplateAndGameObject(
        HavenLocationEventTemplate templateArg, GameObject objectArg)
    {
        Setup(templateArg, objectArg);
    }

    public SerializableDataHavenLocationEventTemplateAndGameObject(
        SerializableDataHavenLocationEventTemplateAndGameObject templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        valueTemplate = null;
        valueObject = null;
    }

    private void Setup(HavenLocationEventTemplate templateArg, GameObject objectArg)
    {
        valueTemplate = templateArg;
        valueObject = objectArg;
    }

    private void Setup(SerializableDataHavenLocationEventTemplateAndGameObject templateArg)
    {
        valueTemplate = templateArg.valueTemplate;
        valueObject = templateArg.valueObject;
    }

    #endregion

}
