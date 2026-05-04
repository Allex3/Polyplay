export type GameComment = {
  id: number
  gameId: number
  username: string
  commentBody: string
}

export function createGameComment(gameComment: Partial<GameComment> = {}): GameComment {
  return {
    id: gameComment.id ?? -1,
    gameId: gameComment.gameId ?? -1,
    username: gameComment.username ?? '0',
    commentBody: gameComment.commentBody ?? 'Bug',
  }
}
