import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ItemsService } from '../../services/items.service';

@Component({
  selector: 'app-items',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.scss']
})
export class ItemsComponent implements OnInit {
  items: any[] = [];

  constructor(private itemsService: ItemsService) {}

  ngOnInit() {
    this.itemsService.getItems().subscribe(data => {
      this.items = data;
    });
  }
}
