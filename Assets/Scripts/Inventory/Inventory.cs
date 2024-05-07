using System.Collections.Generic;

public struct InventoryEntry {
    public int number;
    public InventoryItem inventoryEntry;
}

public class Inventory {
    private LinkedList<InventoryEntry> m_itemList = new();
}

