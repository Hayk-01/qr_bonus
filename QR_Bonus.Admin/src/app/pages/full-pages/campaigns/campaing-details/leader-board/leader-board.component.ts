import { Component, Input, OnInit } from '@angular/core';
import { LeaderBoardModel } from 'app/shared/models/leaderboard.model';
import { PositionPrizeModel } from 'app/shared/models/position-prize.model';
import { HttpService, Urls } from 'app/shared/services/http.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-leader-board',
  templateUrl: './leader-board.component.html',
  styleUrls: ['./leader-board.component.scss']
})
export class LeaderBoardComponent implements OnInit {

  @Input() set campaignId(id: number) {
    this.getData(id);
  }

  public data: Array<LeaderBoardModel> = [];

  public show:boolean = false;

  constructor(
    private http: HttpService,
  ) { }

  ngOnInit(): void {}

  private async getData(id: number) {
    if(!id) return;

    let source = [
      this.http.get<Array<LeaderBoardModel>>(Urls.CampaignLeaderBoardById.replace('campaignId', id.toString())),
      this.http.get<Array<PositionPrizeModel>>(Urls.CampaignLeaderBoardPrizesById.replace('campaignId', id.toString())),
    ]

    let res: any = await forkJoin(source).toPromise();
    if(res) {
      this.data = res[0].value;
      this.data?.forEach(item => {
        item.prize = res[1]?.value.find(x => x.position === item.position)?.prize ?? null;
      })
    }

  }

}


