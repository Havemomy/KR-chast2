using System;

namespace KR_chast2.Entities;

public class Action
{
    public int ActionId { get; set; }
    public int Organizator { get; set; }
    public int Volonter { get; set; }
    public string Cash { get; set; }
    public DateTime Date { get; set; }
}