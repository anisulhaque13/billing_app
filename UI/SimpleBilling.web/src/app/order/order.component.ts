import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Observable, of, BehaviorSubject } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { ApiService } from '../../app/services/api.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css'],
})
export class OrderComponent implements OnInit {
  orderForm: FormGroup;
  categories: any[] = [];
  itemsForCategory: BehaviorSubject<any[]>[] = [];
  orders: any[] = []; // List of saved orders
  errorMessage: string | null = null;
  isLoading = false; // For loading states

  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private http: HttpClient
  ) {
    this.orderForm = this.fb.group({
      orderDate: ['', Validators.required],
      orderDetails: this.fb.array([]),
    });
  }

  ngOnInit(): void {
    this.loadCategories();
    this.loadOrders();
    this.addOrderDetail();
  }

  get orderDetails(): FormArray {
    return this.orderForm.get('orderDetails') as FormArray;
  }

  addOrderDetail(): void {
    const orderDetailGroup = this.fb.group({
      categoryId: ['', Validators.required],
      itemId: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
      price: [0.01, [Validators.required, Validators.min(0.01)]],
    });

    this.orderDetails.push(orderDetailGroup);
    this.itemsForCategory.push(new BehaviorSubject<any[]>([]));
  }

  removeOrderDetail(index: number): void {
    if (this.orderDetails.length > 1) {
      this.orderDetails.removeAt(index);
      this.itemsForCategory.splice(index, 1);
    } else {
      this.errorMessage = 'At least one order detail is required.';
    }
  }

  loadCategories(): void {
    this.isLoading = true;
    this.http
      .get<{ $values?: any[] }>(this.apiService.getUrl('/api/Category'))
      .pipe(
        tap((response) => {
          // Ensure that categories is always an array
          this.categories = Array.isArray(response) ? response : []; // Use nullish coalescing to assign an array
          console.log('Categories loaded:', this.categories);
          this.errorMessage = null;
        }),
        catchError((err) => {
          console.error('Error fetching categories:', err);
          this.categories = []; // Fallback to an empty array
          this.errorMessage = 'Failed to load categories.';
          return of([]);
        })
      )
      .subscribe(() => (this.isLoading = false));
  }

  loadItems(event: Event, index: number): void {
    const selectElement = event.target as HTMLSelectElement;
    const categoryId = selectElement.value;

    if (!categoryId) {
      this.itemsForCategory[index].next([]); // Emit empty array
      return;
    }

    this.isLoading = true;
    this.http
      .get<{ $values?: any[] }>(this.apiService.getUrl(`/api/Item?categoryId=${categoryId}`))
      .pipe(
        tap((response) => {
          // Ensure items is always an array
          const items = Array.isArray(response) ? response : [];
          this.itemsForCategory[index].next(items);
          console.log(`Items loaded for category ${categoryId}:`, items);
        }),
        catchError((err) => {
          console.error(`Error fetching items for category ${categoryId}:`, err);
          this.itemsForCategory[index].next([]); // Emit empty array
          this.errorMessage = 'Failed to load items.';
          return of([]);
        })
      )
      .subscribe(() => (this.isLoading = false));
  }
  loadOrders(): void {
    this.isLoading = true;
    this.http
      .get<{ $values?: any[] }>(this.apiService.getUrl('/api/Order'))
      .pipe(
        tap((response) => {
          this.orders = (response?.$values ?? []).map(order => ({
            ...order,
            orderDetails: Array.isArray(order?.orderDetails?.$values)
              ? order.orderDetails.$values // Extract $values if present
              : []
          }));
          console.log('Orders loaded:', this.orders); // Debug log
          this.errorMessage = null;
        }),
        catchError((err) => {
          console.error('Error fetching orders:', err);
          this.orders = []; // Fallback to an empty array
          this.errorMessage = 'Failed to load orders.';
          return of([]);
        })
      )
      .subscribe(() => (this.isLoading = false));
  }


  onSubmit(): void {
    if (this.orderForm.valid) {
      const orderData = {
        orderDate: this.orderForm.value.orderDate,
        orderDetails: this.orderDetails.value.map((detail: any, index: number) => {
          const category = this.categories.find((c) => c.id === detail.categoryId);
          const categoryName = category?.name || 'Unknown Category';

          const items = this.itemsForCategory[index]?.getValue() || [];
          const item = items.find((i) => i.id === detail.itemId);
          const itemName = item?.name || 'Unknown Item';

          return {
            ...detail,
            categoryName,
            itemName,
          };
        }),
      };

      this.isLoading = true;
      this.http.post(this.apiService.getUrl('/api/Order'), orderData).subscribe({
        next: () => {
          alert('Order saved successfully!');
          this.loadOrders();
          this.resetForm();
        },
        error: (err) => {
          console.error('Error saving order:', err);
          this.errorMessage = err.error?.message || 'An error occurred while saving the order.';
        },
        complete: () => (this.isLoading = false),
      });
    } else {
      this.errorMessage = 'Please fill in all required fields.';
      this.markAllFieldsAsTouched();
    }
  }

  resetForm(): void {
    this.orderForm.reset();
    this.orderDetails.clear();
    this.itemsForCategory = [];
    this.addOrderDetail();
    this.errorMessage = null;
  }

  private markAllFieldsAsTouched(): void {
    this.orderForm.markAllAsTouched();
    this.orderDetails.controls.forEach((control) => control.markAsTouched());
  }
}
