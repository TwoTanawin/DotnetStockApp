import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms'
import { MatIconButton, MatButton } from '@angular/material/button'
import { MatIcon } from '@angular/material/icon'
import { MatInput } from '@angular/material/input'
import { MatFormField, MatLabel, MatSuffix } from '@angular/material/form-field';
import { MatCard, MatCardImage, MatCardHeader, MatCardTitle, MatCardContent, MatCardActions } from '@angular/material/card'
import { Meta } from '@angular/platform-browser'

// Import user service
import { UserService } from '../../services/user.service'

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrl: './login.component.scss',
    standalone: true,
    imports: [
        MatCard,
        MatCardImage,
        MatCardHeader,
        MatCardTitle,
        MatCardContent,
        MatFormField,
        MatLabel,
        MatInput,
        MatIcon,
        MatSuffix,
        MatIconButton,
        MatCardActions,
        MatButton,
        ReactiveFormsModule
    ],
})
export class LoginComponent implements OnInit {
  // Form Validation
  loginForm!: FormGroup
  submitted: boolean = false

  // Variables สำหรับรับค่าจากฟอร์ม
  userData = {
    username: '',
    password: '',
  }

  // สร้างตัวแปรเก็บข้อมูลการ Login
  userLogin = {
    "username": "",
    "email": "",
    "role": "",
    "token": ""
  }

  // สำหรับซ่อนแสดง password
  hide = true

  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private meta: Meta,
    private http: UserService,
) {}

  ngOnInit() {

    // กำหนด Meta Tag description
    this.meta.addTag({ name: 'description', content: 'Login page for Stock Management' })

    // กำหนดค่าให้กับ Form
    this.loginForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(3)]], // iamsamit
      password: ['', [Validators.required, Validators.minLength(8)]], // Samit@1234
    })
  }

  // ฟังก์ชัน Submit สำหรับ Login
  submitLogin() {
    this.submitted = true
    if (this.loginForm.invalid) {
      return
    } else {
      this.userData.username = this.loginForm.value.username
      this.userData.password = this.loginForm.value.password

      console.log(this.userData)

      // เรียกใช้งาน Service สำหรับ Login
      this.http.Login(this.userData).subscribe({
        next: (data: any) => {
          console.log(data)
        },
        error: (error) => {
          console.log(error)
        }
      })

    }
  }

  // สำหรับลิงก์ไปหน้า Register
  onClickRegister() {
    this.router.navigate(['/register'])
  }
}