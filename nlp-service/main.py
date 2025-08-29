from fastapi import FastAPI
from pydantic import BaseModel
from datetime import datetime, timedelta
from vaderSentiment.vaderSentiment import SentimentIntensityAnalyzer

app = FastAPI()
analyzer = SentimentIntensityAnalyzer()

class AnalyzeReq(BaseModel):
    source: str       # "reddit" | "twitter" | "news"
    query: str
    lookback_minutes: int = 60
    max_posts: int = 200

class Point(BaseModel):
    t: datetime
    v: float

class AnalyzeResp(BaseModel):
    source: str
    score: float
    ema5m: float
    series: list[Point]

@app.post("/sentiment/analyze", response_model=AnalyzeResp)
def analyze(req: AnalyzeReq):
    # TODO: fetch posts from Reddit/Twitter/News (e.g., PRAW/Twitter API/newsapi)
    # For now fake a series and compute EMA over it.
    now = datetime.utcnow()
    series = [Point(t=now - timedelta(minutes=i*2), v=(i%5 - 2)/2.0) for i in range(20)][::-1]

    # Simple EMA
    k = 2/(5 + 1)
    ema = series[0].v
    for p in series[1:]:
        ema = p.v * k + ema * (1 - k)

    return AnalyzeResp(source=req.source, score=series[-1].v, ema5m=ema, series=series)
