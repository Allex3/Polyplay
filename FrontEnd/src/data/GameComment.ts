export type GameComment = {
  id: number
  gameId: number
  userId: number
  body: string //REMINDER: THE NAMES HERE HAVE TO BE THE SAME AS IN THE API!!!
}

export function createGameComment(gameComment: Partial<GameComment> = {}): GameComment {
  return {
    id: gameComment.id ?? -1,
    gameId: gameComment.gameId ?? -1,
    userId: gameComment.userId ?? -1,
    body: gameComment.body ?? '',
  }
}
