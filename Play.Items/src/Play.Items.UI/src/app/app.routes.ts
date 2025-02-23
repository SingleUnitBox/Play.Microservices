import { Routes } from '@angular/router';
import { ItemsComponent } from './components/items/items.component';

export const routes: Routes = [
  { path: '', redirectTo: '/items', pathMatch: 'full' },  // Default route to /items
  { path: 'items', component: ItemsComponent },           // Route to display items
];
