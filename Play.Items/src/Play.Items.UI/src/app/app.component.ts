import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ItemsComponent } from './components/items/items.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ItemsComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  standalone: true
})
export class AppComponent {
  title = 'Play.Items.UI';
}
