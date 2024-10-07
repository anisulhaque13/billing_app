import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { Category } from '../../models/category.model';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrl: './category.component.css'
})
export class CategoryComponent {

  http = inject(HttpClient);
  categoryForm = new FormGroup({
    name: new FormControl('', Validators.required)  // Form control with validation
  });

  categories$ = this.getCategories();
  selectedCategory: Category | null = null;

  onFormSubmit() {
    const formData = {
      name: this.categoryForm.value.name
    };

    if (this.selectedCategory) {
      this.http.put(`https://localhost:7188/api/Category/${this.selectedCategory.id}`, formData)
        .subscribe({
          next: () => {
            this.categories$ = this.getCategories();
            this.resetForm();
          }
        });
    } else {
      this.http.post('https://localhost:7188/api/Category', formData)
        .subscribe({
          next: () => {
            this.categories$ = this.getCategories();
            this.resetForm();
          }
        });
    }
  }

  onEdit(category: Category) {
    this.selectedCategory = category;
    this.categoryForm.patchValue({
      name: category.name
    });
  }

  onDelete(id: string) {
    this.http.delete(`https://localhost:7188/api/Category/${id}`)
      .subscribe({
        next: () => {
          alert('Item deleted');
          this.categories$ = this.getCategories();
        }
      });
  }

  // Make resetForm public so it's accessible in the template
  public resetForm() {
    this.categoryForm.reset();
    this.selectedCategory = null;
  }

  private getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>('https://localhost:7188/api/Category');
  }
}
