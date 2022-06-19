import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Project } from 'src/app/models/project';
import { ProjectService } from 'src/app/services/project.service';

@Component({
  selector: 'app-project-dialog',
  templateUrl: './project-dialog.component.html',
  styleUrls: ['./project-dialog.component.css']
})
export class ProjectDialogComponent implements OnInit {
  formGroup? : FormGroup;
  constructor(private formBuilder : FormBuilder,
              private projectService : ProjectService,
              private dialogRef : MatDialogRef<ProjectDialogComponent>,
              private router: Router,
              @Inject(MAT_DIALOG_DATA) public data : Project) { }

  ngOnInit(): void {
    this.formGroup = this.formBuilder.group({
      title: [this.data ? this.data.title : "", [Validators.required, Validators.maxLength(50)]],
      description: [this.data ? this.data.description : "", [Validators.required, Validators.maxLength(200)]]
    });
  }

  createProject() {
    this.projectService.create(this.formGroup?.value).subscribe({
      next: () => {
        this.formGroup?.reset();
        this.dialogRef.close();

        this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => 
        this.router.navigate(['projects']));
      },
      error: () => {
        alert("Error when trying to create a new project. Verify the title and description and try again later.");
        this.formGroup?.reset();
      }
    });
  }

  editProject() {
    this.projectService.put(this.data.index, this.formGroup?.value).subscribe({
      next: () => {
        this.formGroup?.reset();
        this.dialogRef.close();

        this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => 
        this.router.navigate(['projects']));
      },
      error: () => {
        alert("Error when trying to edit a project. Verify the title and description and try again later.");
        this.formGroup?.reset();
      }
    })
  }
}
