import { Component, Input, ViewEncapsulation } from '@angular/core';
import { Member } from 'src/app/models/member';

@Component({
  selector: 'app-members-card',
  templateUrl: './members-card.component.html',
  styleUrls: ['./members-card.component.css'],
})
export class MembersCardComponent {
  @Input() member: Member | undefined; //= {} as Member;
}
