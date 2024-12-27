import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from "@angular/common/http";
import { Observable, of } from "rxjs";
import { map, tap, catchError } from 'rxjs/operators';
import { Category } from '../../models/category.model';
import { ApiService } from '../../app/services/api.service';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrl: './category.component.css'
})
export class CategoryComponent implements OnInit {
  categoryForm = new FormGroup({
    name: new FormControl('', Validators.required)
  });

  categories$: Observable<Category[]> = of([]); // Initialize with an empty observable
  selectedCategory: Category | null = null;

  constructor(private apiService: ApiService, private http: HttpClient) { }

  ngOnInit() {
    this.categories$ = this.getCategories(); // Initialize with actual data in ngOnInit
  }

  onFormSubmit() {
    const formData = { name: this.categoryForm.value.name };

    if (this.selectedCategory) {
      this.http.put(this.apiService.getUrl(`/api/Category/${this.selectedCategory.id}`), formData)
        .subscribe({
          next: () => {
            this.categories$ = this.getCategories(); // Refresh categories
            this.resetForm();
          },
          error: (err) => {
            console.error('Error updating category:', err);
            alert('Failed to update category.');
          }
        });
    } else {
      this.http.post(this.apiService.getUrl('/api/Category'), formData)
        .subscribe({
          next: () => {
            this.categories$ = this.getCategories(); // Refresh categories
            this.resetForm();
          },
          error: (err) => {
            console.error('Error creating category:', err);
            alert('Failed to create category.');
          }
        });
    }
  }

  onEdit(category: Category) {
    this.selectedCategory = category;
    this.categoryForm.patchValue({ name: category.name });
  }

  onDelete(id: string) {
    this.http.delete(this.apiService.getUrl(`/api/Category/${id}`))
      .subscribe({
        next: () => {
          alert('Item deleted');
          this.categories$ = this.getCategories(); // Refresh categories
        },
        error: (err) => {
          console.error('Error deleting category:', err);
          alert('Failed to delete category.');
        }
      });
  }

  public resetForm() {
    this.categoryForm.reset();
    this.selectedCategory = null;
  }

  private getCategories(): Observable<Category[]> {
    return this.http.get<any>(this.apiService.getUrl('/api/Category'))
      .pipe(
        map((response) => response.$values || []), // Extract $values array
        catchError((err) => {
          console.error('Error fetching categories:', err);
          alert('Failed to fetch categories.');
          return of([]); // Return an empty array on error
        })
      );
  }

}
