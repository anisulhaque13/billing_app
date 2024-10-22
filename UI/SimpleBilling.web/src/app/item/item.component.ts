import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from "@angular/common/http";
import { from, Observable } from "rxjs";
import { Item } from '../../models/item.model';
import { Category } from '../../models/category.model';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrl: './item.component.css'
})
export class ItemComponent {

  http = inject(HttpClient);
  itemForm = new FormGroup({
    name: new FormControl('', Validators.required),  // Form control with validation
    categoryId: new FormControl('', Validators.required),
    price: new FormControl('', Validators.required),
    unit: new FormControl('', Validators.required),
  });

  items$ = this.getItems();
  categories$ = this.getCategories();
  selectedItem: Item | null = null;

  onFormSubmit() {
    if (this.itemForm.invalid) {
      console.log('Form is invalid:', this.itemForm.value);
      return; // Exit if form is invalid
    }

    // Ensure categoryId is passed as an integer
    const formData = {
      name: this.itemForm.value.name,
      categoryId: this.itemForm.value.categoryId, 
      unit: this.itemForm.value.unit,
      price: this.itemForm.value.price // Ensure price is a decimal/float
    };

    console.log('Form data:', formData);  // Log form data to inspect

    if (this.selectedItem) {
      // PUT request for editing an item
      this.http.put(`https://localhost:7188/api/Item/${this.selectedItem.id}`, formData)
        .subscribe({
          next: () => {
            this.items$ = this.getItems();
            this.resetForm();
          },
          error: (error) => {
            console.error('Error updating item:', error);
          }
        });
    } else {
      // POST request for adding a new item
      this.http.post('https://localhost:7188/api/Item', formData)
        .subscribe({
          next: () => {
            this.items$ = this.getItems();
            this.resetForm();
          },
          error: (error) => {
            console.error('Error adding item:', error);
          }
        });
    }
  }


  onEdit(item: Item) {
    this.selectedItem = item;
    this.itemForm.patchValue({
      name: item.name
    });
  }

  onDelete(id: string) {
    this.http.delete(`https://localhost:7188/api/Item/${id}`)
      .subscribe({
        next: () => {
          alert('Item deleted');
          this.items$ = this.getItems();
        }
      });
  }

  // Make resetForm public so it's accessible in the template
  public resetForm() {
    this.itemForm.reset();
    this.selectedItem = null;
  }

  private getItems(): Observable<Item[]> {
    return this.http.get<Item[]>('https://localhost:7188/api/Item');
  }
  private getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>('https://localhost:7188/api/Category');
  }
}
