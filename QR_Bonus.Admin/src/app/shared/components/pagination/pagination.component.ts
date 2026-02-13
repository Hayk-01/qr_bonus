import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { PaginationConfig } from './pagination.config';
import { PaginationModel } from 'app/shared/models/pagination.model';
import { EventsEmitterService } from 'app/shared/services/events-emitter.service';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss']
})
export class PaginationComponent implements OnInit, OnDestroy {
  private page: number | null = null;

  public pagination: PaginationModel = {} as PaginationModel;

  public pages: Array<number> = [];

  public filters = {
    skip: PaginationConfig.skip,
    take: PaginationConfig.take,
  }

  public show:boolean = false;

  private destroyed$: Subject<boolean> = new Subject<boolean>();

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private eventsEmitterService: EventsEmitterService,
    private cdr: ChangeDetectorRef
  ) {
  }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe((queryParams: Params) => {

      if(queryParams.hasOwnProperty('skip')) {
        this.filters.skip = +queryParams['skip'];
        this.pagination.pageNumber = +queryParams['skip'];
        this.pagination.pageNumber = +queryParams['skip'];
      } else {
        this.filters.skip = PaginationConfig.skip;
        this.pagination.pageNumber = PaginationConfig.skip;
      }

    });

    this.eventsEmitterService.paginationDataEmiter
    .pipe(takeUntil(this.destroyed$))
    .subscribe((res: PaginationModel) => {
      if(res) {
        this.pagination = res;
        if(this.page !== null) {
          this.pagination.pageNumber = this.page;
        }
        this.show = this.pagination.totalPages > 1;
        this.cdr.detectChanges();
      }
    })

  }

  public goto(page: any) {
    if(page == "" || page === undefined || page == null || page == 0) {
      return;
    }

    if(page > this.pagination.totalPages) {
      this.pagination.pageNumber = 1;
      this.filters.skip = 1;
    } else {
      this.filters.skip = page;
    }

    this.router.navigate([], {
      relativeTo: this.activatedRoute,
      queryParams: this.filters,
      queryParamsHandling: 'merge',
    })
  }

  public arrowPress(value: boolean) {
    if(value && this.filters.skip < this.pagination.totalPages) {
      this.filters.skip = Number(this.filters.skip) + 1;
    }
    if(!value && this.filters.skip > 1) {
      this.filters.skip = Number(this.filters.skip) - 1;
    }
    this.router.navigate([], {
      relativeTo: this.activatedRoute,
      queryParams: this.filters,
      queryParamsHandling: 'merge',
    })
  }

  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.unsubscribe();
  }

}
