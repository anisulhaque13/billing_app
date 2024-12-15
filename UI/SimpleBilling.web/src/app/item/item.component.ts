import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from "@angular/common/http";
import { Observable, of } from "rxjs";
import { catchError } from "rxjs/operators";
import { Item } from '../../models/item.model';
import { Category } from '../../models/category.model';
import { ApiService } from '../../app/services/api.service'; // Import ApiService

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrl: './item.component.css'
})
export class ItemComponent {

  private http = inject(HttpClient);
  private apiService = inject(ApiService);

  itemForm = new FormGroup({
    name: new FormControl('', Validators.required),
    categoryId: new FormControl('', Validators.required),
    price: new FormControl('', [Validators.required, Validators.min(0)]), // Ensure price is positive
    unit: new FormControl('', Validators.required),
  });

  items$: Observable<Item[]> = of([]); // Initialize with an empty observable
  categories$: Observable<Category[]> = of([]);
  selectedItem: Item | null = null;

  constructor() {
    this.loadData();
  }

  private loadData() {
    // Fetch items and categories on load
    this.items$ = this.getItems();
    this.categories$ = this.getCategories();
  }

  onFormSubmit() {
    if (this.itemForm.invalid) {
      console.error('Form is invalid:', this.itemForm.value);
      return; // Exit if form is invalid
    }

    const formData = {
      name: this.itemForm.value.name,
      categoryId: this.itemForm.value.categoryId,
      unit: this.itemForm.value.unit,
      price: this.itemForm.value.price
    };

    if (this.selectedItem) {
      // PUT request to update item
      this.http.put(this.apiService.getUrl(`/api/Item/${this.selectedItem.id}`), formData)
        .subscribe({
          next: () => {
            this.items$ = this.getItems(); // Refresh items
            this.resetForm();
          },
          error: (error) => {
            console.error('Error updating item:', error);
            alert('Failed to update the item.');
          }
        });
    } else {
      // POST request to add new item
      this.http.post(this.apiService.getUrl('/api/Item'), formData)
        .subscribe({
          next: () => {
            this.items$ = this.getItems(); // Refresh items
            this.resetForm();
          },
          error: (error) => {
            console.error('Error adding item:', error);
            alert('Failed to add the item.');
          }
        });
    }
  }

  onEdit(item: Item) {
    this.selectedItem = item;
    this.itemForm.patchValue({
      name: item.name,
      categoryId: item.categoryId,
      price: item.price,
      unit: item.unit
    });
  }

  onDelete(id: string) {
    this.http.delete(this.apiService.getUrl(`/api/Item/${id}`))
      .subscribe({
        next: () => {
          alert('Item deleted');
          this.items$ = this.getItems(); // Refresh items
        },
        error: (error) => {
          console.error('Error deleting item:', error);
          alert('Failed to delete the item.');
        }
      });
  }

  public resetForm() {
    this.itemForm.reset();
    this.selectedItem = null;
  }

  private getItems(): Observable<Item[]> {
    return this.http.get<Item[]>(this.apiService.getUrl('/api/Item'))
      .pipe(
        catchError((error) => {
          console.error('Error fetching items:', error);
          alert('Failed to fetch items.');
          return of([]); // Return an empty array on error
        })
      );
  }

  private getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(this.apiService.getUrl('/api/Category'))
      .pipe(
        catchError((error) => {
          console.error('Error fetching categories:', error);
          alert('Failed to fetch categories.');
          return of([]); // Return an empty array on error
        })
      );
  }
}
