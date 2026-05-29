export type GameComment = {
  id: number
  gameId: number
  userName: string
  body: string //REMINDER: THE NAMES HERE HAVE TO BE THE SAME AS IN THE API!!!
}

export function createGameComment(gameComment: Partial<GameComment> = {}): GameComment {
  return {
    id: gameComment.id ?? -1,
    gameId: gameComment.gameId ?? -1,
    userName: gameComment.userName ?? '',
    body: gameComment.body ?? '',
  }
}
