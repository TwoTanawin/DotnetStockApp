import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core'
import { MatMenuTrigger, MatMenu, MatMenuItem } from '@angular/material/menu'
import { MatIcon } from '@angular/material/icon'
import { MatIconButton } from '@angular/material/button'
import { MatToolbar, MatToolbarRow } from '@angular/material/toolbar'

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrl: './header.component.scss',
    standalone: true,
    imports: [
      MatToolbar, 
      MatToolbarRow, 
      MatIconButton, 
      MatIcon, 
      MatMenuTrigger, 
      MatMenu, 
      MatMenuItem
    ]
})
export class HeaderComponent implements OnInit {

  // สร้างตัวแปรไว้เก็บข้อมูลผู้ใช้งานที่ Login
  userProfile: any = {
    "username": "",
    "email": "",
    "role": ""
  }

  @Output() sidenavToggle = new EventEmitter<void>();
  @Input() isOpened?: boolean

  pageName: string = 'Stock'
  version = '17.3'

  constructor(
    
  ) {
  
  }

  ngOnInit(): void {
  }

  onToggleSidenav() {
    this.sidenavToggle.emit()
  }

  onClickSignout() {
    window.location.href = '/login'
  }

}