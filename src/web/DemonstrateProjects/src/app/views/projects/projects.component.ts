import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Project } from 'src/app/models/project';
import { ProjectService } from 'src/app/services/project.service';
import { ProjectDialogComponent } from './project-dialog/project-dialog.component';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css']
})
export class ProjectsComponent implements OnInit {

  constructor(private projectService : ProjectService, 
              private dialog : MatDialog,
              private router : Router) { }

  ngOnInit(): void {
    this.getAll();
  }

  private getAll() : void {
    this.projectService.getAll().subscribe({
      next: (x) => {
        this.projects = x;
      },
      error: () => {
        alert("Error when trying to load all projects. Please, try again later.")
      }
    });
  }

  openNewProjectDialog() : void {
    const dialogRef = this.dialog.open(ProjectDialogComponent, {
      width: '70%',
      height: 'fit-content'
    });
  }

  changeMainProject(index : Number, event : Event) {
    /* Changing Route */
    this.router.navigate(['/projects/' + index]);
    
    /* Styling selected project */
    const article = (document.getElementsByClassName('project__card--selected')[0]);
    if (article) {
      article.classList.replace("project__card--selected", "project__card");
    }

    const selected = (event.currentTarget as Element);
    if (selected) {
      if (selected.classList.contains("project__card")) {
        selected.classList.replace("project__card", "project__card--selected");
      } else {
        selected.parentElement?.classList.replace("project__card", "project__card--selected");
      }
    }
  }
  
  deleteProject(index: Number) {
    let sure = confirm("Are you sure?");
    if (sure) {
      this.projectService.delete(index).subscribe({
        next: () => {
          this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => 
          this.router.navigate(['projects'])
        );
        },
        error: () => {
          alert("Error when trying to delete the project. May it is unexistent or unavaible.");
        }
      });
    } else {
      alert("Uh!");
    }
  }

  projects? : Project[];
}
