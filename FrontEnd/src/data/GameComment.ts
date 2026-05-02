export type GameComment = {
  commentId: number
  gameId: number
  username: string
  commentBody: string
}

export function createGameComment(gameComment: Partial<GameComment> = {}): GameComment {
  return {
    commentId: gameComment.commentId ?? -1,
    gameId: gameComment.gameId ?? -1,
    username: gameComment.username ?? '0',
    commentBody: gameComment.commentBody ?? 'Bug',
  }
}
