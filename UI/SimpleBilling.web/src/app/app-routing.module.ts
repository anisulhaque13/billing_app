import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CategoryComponent } from './category/category.component';
import { ItemComponent } from './item/item.component';
import { OrderComponent } from './order/order.component';

const routes: Routes = [
  { path: 'category-component', component: CategoryComponent },
  { path: 'item-component', component: ItemComponent },
  { path: 'order-component', component: OrderComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
