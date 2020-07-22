using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private int width; // x
    [SerializeField]
    private int height; // y
    [SerializeField]
    private int depth; // z

    [SerializeField, Range(0, 100)]
    private int randomFillPercent; // 블록 생성 비율
    [SerializeField, Range(0, 10)]
    private float weight; // 층이 쌓일수록 생성 비율을 조절하는 변수
    [SerializeField, Range(0, 26)]
    private int neighbourCount; // 이웃하는 블록수 제한
    [SerializeField, Range(0, 100)]
    private int smoothCount; // 스무딩 횟수

    public GameObject cubePrefab;
    public Material sandMaterial;
    public Material grassMaterial;

    private GameObject[,,] cubes;

    public Transform mapTrans;

    private int[,,] map;

    // BSP 알고리즘을 수행하는 트리 클래스
    private class BSP
    {
        private int depth = 0; // 노드 깊이
        private bool isLeaf = false;

        // 방(언덕)의 중심 좌표
        private float x;
        private float y;

        // 방(언덕)의 가로, 세로, 높이
        private float row;
        private float col;
        private float height;

        private BSP left;
        private BSP right;

        public BSP(float x, float y, float row, float col, float height)
        {
            this.x = x;
            this.y = y;
            this.row = row;
            this.col = col;
            this.height = height;
        }

        // 트리를 구현하는 클래스 내부 메소드
        // leaf node에 방(언덕)의 데이터가 포함된다
        public void MakeChild(int depth)
        {
            this.depth = depth;

            if (this.depth == 3)
            {
                this.isLeaf = true;
                NewRoom();
                return;
            }

            if (this.depth % 2 == 0)
            {
                float leftRow = UnityEngine.Random.Range(row * 0.3f, row * 0.7f);
                float rightRow = row - leftRow;
                float leftX = x - rightRow / 2;
                float rightX = x + leftRow / 2;

                this.left = new BSP(leftX, y, leftRow, col, height);
                this.right = new BSP(rightX, y, rightRow, col, height);
            }
            else
            {
                float downCol = UnityEngine.Random.Range(col * 0.3f, col * 0.7f);
                float upCol = col - downCol;
                float downY = y - upCol / 2;
                float upY = y + downCol / 2;

                this.left = new BSP(x, downY, row, downCol, height);
                this.right = new BSP(x, upY, row, upCol, height);
            }

            this.left.MakeChild(this.depth + 1);
            this.right.MakeChild(this.depth + 1);
        }

        // 방(언덕)의 데이터를 무작위로 설정하는 클래스 내부 메소드
        void NewRoom()
        {
            float oldRow = this.row;
            float oldCol = this.col;
            this.height = UnityEngine.Random.Range(this.height * 0.3f, this.height * 0.7f);
            this.row = UnityEngine.Random.Range(this.row * 0.3f, this.row * 0.7f);
            this.col = UnityEngine.Random.Range(this.col * 0.3f, this.col * 0.7f);
            this.x = this.x + UnityEngine.Random.Range(-(oldRow-this.row)/2, (oldRow-this.row)/2);
            this.y = this.y + UnityEngine.Random.Range(-(oldCol-this.col)/2, (oldCol-this.col)/2);
        }

        // 모든 leaf node를 리스트로 반환하는 클래스 내부 메소드
        public List<BSP> GetLeafNode()
        {
            List<BSP> ret = new List<BSP>();
            if (this.isLeaf)
            {
                ret.Add(this);
                return ret;
            }
            List<BSP> leftRet = (this.left).GetLeafNode();
            List<BSP> rightRet = (this.right).GetLeafNode();
            ret.AddRange(leftRet);
            ret.AddRange(rightRet);

            return ret;
        }

        // 방(언덕)의 데이터를 딕셔너리로 반환하는 클래스 내부 메소드
        public Dictionary<string, float> GetValues()
        {
            Dictionary<string, float> ret = new Dictionary<string, float>();
            ret.Add("x", this.x);
            ret.Add("y", this.y);
            ret.Add("row", this.row);
            ret.Add("col", this.col);
            ret.Add("height", this.height);

            return ret;
        }
    }

    void Start()
    {
        map = new int[width, height, depth];
        cubes = new GameObject[width, height, depth];
        GenerateMap();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StopCoroutine(Draw());
            GenerateMap();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            SmoothMap();
            smoothCount++;
            StartCoroutine(Draw());
        }
    }

    // 랜덤 맵 생성 메소드
    void GenerateMap()
    {
        RandomFillMap();
        BSP2Map();
        BSP2Map();

        for (int i = 0; i < smoothCount; i++)
        {
            SmoothMap();
        }
        StartCoroutine(Draw());
    }

    // 랜덤으로 블록을 생성하는 메소드
    void RandomFillMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float _x = Mathf.Abs(x - width / 2) / (float)(width / 2);
                float _z = Mathf.Abs(z - depth / 2) / (float)(depth / 2);
                float weight = Mathf.Pow(_x, 2) + Mathf.Pow(_z, 2) > 1 ? 0.1f : 1f;
                map[x, 0, z] = (UnityEngine.Random.Range(0, 100) < randomFillPercent * weight) ? 1 : 0;
            }
        }
        for (int y = 1; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    map[x, y, z] = (map[x, y-1, z] == 1 && UnityEngine.Random.Range(0, 100) < randomFillPercent - weight * y) ? 1 : 0;
                }
            }
        }
    }

    // 굴곡을 완만하게 다듬는 메소드
    void SmoothMap()
    {
        for (int y = height-1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    int neighbourWallTiles = GetSurroundingWallCount(x, y, z);

                    if (neighbourWallTiles > neighbourCount)
                        map[x, y, z] = 1;
                    else if (neighbourWallTiles < neighbourCount)
                        map[x, y, z] = 0;
                }
            }
        }
        for (int y = 0; y < height; y++)
        {
            for (int z = 1; z < depth - 1; z++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    if (map[x, y, z] == 0)
                    {
                        if ((map[x+1, y, z] == 1 && map[x-1, y, z] == 1) || (map[x, y, z+1] == 1 && map[x, y, z-1] == 1))
                        {
                            map[x, y, z] = 1;
                        }
                    }
                }
            }
        }
    }

    // 주변의 블록 개수를 반환하는 메소드
    int GetSurroundingWallCount(int gridX, int gridY, int gridZ)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                for (int neighbourZ = gridZ - 1; neighbourZ <= gridZ + 1; neighbourZ++)
                {
                    if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height && neighbourZ >= 0 && neighbourZ < depth)
                    {
                        if (neighbourX != gridX || neighbourY != gridY || neighbourZ != gridZ)
                        {
                            wallCount += map[neighbourX, neighbourY, neighbourZ];
                        }
                    }
                }
            }
        }

        return wallCount;
    }

    // BSP 알고리즘으로 언덕을 생성한 후 맵에 적용하는 메소드
    void BSP2Map()
    {
        Dictionary<int, Dictionary<string, float>> roomDatas = BSPGenerate(width/2, depth/2, width, depth, height);
        
        for (int i = 0; i < roomDatas.Count; i++)
        {
            int centerX = Mathf.RoundToInt(roomDatas[i]["x"]);
            int centerY = Mathf.RoundToInt(roomDatas[i]["y"]);
            int row = Mathf.RoundToInt(roomDatas[i]["row"]);
            int col = Mathf.RoundToInt(roomDatas[i]["col"]);
            int height = Mathf.RoundToInt(roomDatas[i]["height"]);

            int gridX = Mathf.RoundToInt(row/2);
            int gridY = Mathf.RoundToInt(col/2);
            for (int h = 0; h < height; h++)
            {
                for (int x = centerX-gridX; x < centerX+gridX; x++)
                {
                    for (int y = centerY-gridY; y < centerY+gridY; y++)
                    {
                        map[x, h, y] = 1;
                    }
                }
            }
        }
    }

    // BSP 알고리즘을 수행하고 결과값을 반환하는 메소드
    Dictionary<int, Dictionary<string, float>> BSPGenerate(float x, float y, int width, int depth, int height)
    {
        BSP bsp = new BSP(x, y, (float)width, (float)depth, (float)height);
        bsp.MakeChild(0);
        List<BSP> rooms = bsp.GetLeafNode();

        Dictionary<int, Dictionary<string, float>> roomDatas = new Dictionary<int, Dictionary<string, float>>();

        int i = 0;
        foreach (BSP room in rooms)
        {
            Dictionary<string, float> values = room.GetValues();
            roomDatas.Add(i, values);
            i++;
        }

        return roomDatas;
    }

    // 블록을 시각화하는 메소드
    IEnumerator Draw()
    {
        if (map != null)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        if (cubes[x, y, z] == null)
                        {
                            GameObject newCube = Instantiate(cubePrefab, mapTrans) as GameObject;
                            cubes[x, y, z] = newCube;
                        }
                        Vector3 pos = new Vector3(-width / 2 + x + .5f, (-height / 2 + y + .5f)/2, -depth / 2 + z + .5f);
                        cubes[x, y, z].transform.position = pos;
                        if (map[x, y, z] == 1)
                        {
                            if (y == height-1 || map[x, y+1, z] == 0)
                            {
                                cubes[x, y, z].GetComponent<MeshRenderer>().material = grassMaterial;
                            }
                            else
                            {
                                cubes[x, y, z].GetComponent<MeshRenderer>().material = sandMaterial;
                            }
                            cubes[x, y, z].SetActive(true);
                        }
                        else
                        {
                            cubes[x, y, z].SetActive(false);
                        }
                    }
                }
                yield return null;
            }
        }
    }
}
