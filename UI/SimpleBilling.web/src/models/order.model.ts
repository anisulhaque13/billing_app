export interface Order {
  id: string; // Order Id (Guid)
  orderDate: Date; // Date of the order
  orderDetails: OrderDetail[]; // Array of OrderDetail items
}

export interface OrderDetail {
  id: string; // Order Detail Id (Guid)
  itemId: string; // Item Id (Guid)
  itemName: string; // Item Name
  categoryId: string; // Category Id (Guid)
  categoryName: string; // Category Name
  quantity: number; // Quantity of the item
  price: number; // Price of the item
}
