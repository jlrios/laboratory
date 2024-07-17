
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  data = { "X999": ["X123", "X456", "789"] };
  entries: [string, string[]][];

  ngOnInit() {
    this.entries = Object.entries(this.data);
  }
}
<div *ngFor="let entry of entries">
  <p>Key: {{ entry[0] }}</p>
  <ul>
    <li *ngFor="let item of entry[1]">{{ item }}</li>
  </ul>
</div>
