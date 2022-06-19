import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, ActivatedRouteSnapshot, ParamMap, Router } from '@angular/router';
import { switchMap, tap } from 'rxjs';
import { Project } from 'src/app/models/project';
import { ProjectService } from 'src/app/services/project.service';
import { ProjectDialogComponent } from '../projects/project-dialog/project-dialog.component';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})  
export class ProjectComponent implements OnInit {

  constructor(private route : ActivatedRoute,
              private projectService : ProjectService,
              private dialog : MatDialog,
              private router : Router) { }

  ngOnInit(): void {
    this.route.firstChild?.paramMap.pipe(tap()).subscribe(params => {
      this.id = params.get('id')!;
    
      if (this.id) {
        this.projectService.get(Number(this.id)).subscribe({
          next: (x) => {
            this.project = x;
          },
          error: () => {
            alert("Error when trying to load project. Try again later or reload the page.");
          }
        });
      }
    });
  }

  changeProjectImg() {
    const file = (document.getElementById('projectImgInput') as HTMLInputElement).files![0];
    console.log(file);
    this.projectService.changeImg(file, this.id!).subscribe({
      next: () => {
        alert("Successfully saved image!");
      },
      error: () => {
        alert("Error when changing image. Verify the size and the type of the img.")
      }
    });
  }

  openEditProjectDialog() : void {
    const dialogRef = this.dialog.open(ProjectDialogComponent, {
      width: '70%',
      height: 'fit-content',
      data: this.project
    });
  }

  private id? : string;
  project? : Project;
}
